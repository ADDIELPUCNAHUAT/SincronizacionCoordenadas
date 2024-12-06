using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SincronizacionCoordenadas.Data.db;
using SincronizacionCoordenadas.Entity;
using SincronizacionCoordenadas.Entity.dto;
namespace SincronizacionCoordenadas.Service
{
    public class TrackingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TrackingService> _logger;
        private readonly IConfiguration _configuration;
        private readonly TrackingContext _context;

        public TrackingService(ILogger<TrackingService> logger, IConfiguration configuration, TrackingContext context)
        {
            _httpClient = new HttpClient();
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public async Task<(string UserIdGuid, string SessionId)> LoginService()
        {
            try
            {
                const string loginUrl = "https://api.3dtracking.net/api/v1.0/Authentication/UserAuthenticate?UserName=ROBYGPS.API&Password=TELCEL.2024";

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(loginUrl);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                using var jsonDocument = JsonDocument.Parse(responseContent);
                var root = jsonDocument.RootElement;

                var userIdGuid = root.GetProperty("Result").GetProperty("UserIdGuid").GetString();
                var sessionId = root.GetProperty("Result").GetProperty("SessionId").GetString();

                return (userIdGuid, sessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión");
                throw;
            }
        }

        public async Task FetchAndSaveLatestPositions(string unitUid = "*")
        {
            try
            {
                // Obtener los últimos registros por vehículo
               var lastRegisteredTimes = await _context.VehicleTrackings
                .GroupBy(v => v.Uid)
                .AsNoTracking()
                .Select(g => new { Uid = g.Key, LastReportedTime = g.Max(v => v.LastReportedTimeUTC) })
                .ToDictionaryAsync(x => x.Uid, x => x.LastReportedTime);

                // Calcular el timestamp para el filtro (3 minutos atrás desde la última posición)
                DateTime filterTime = DateTime.UtcNow.AddMinutes(-3);

                // Formatear la fecha usando la cultura en-US para asegurar que los meses estén en inglés
                string lastDateReceivedUtc = filterTime.ToString("dd MMM yyyy HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);

                var (userIdGuid, sessionId) = await LoginService();

                var apiUrl = $"https://api.3dtracking.net/api/v1.0/Units/LatestPositionsList?" +
                    $"UserIdGuid={userIdGuid}" +
                    $"&SessionId={sessionId}" +
                    $"&UnitUid={unitUid}" +
                    $"&LastDateReceivedUtc={Uri.EscapeDataString(lastDateReceivedUtc)}";

                _logger.LogInformation("Consultando posiciones desde: {lastDateReceivedUtc}", lastDateReceivedUtc);

                var response = await _httpClient.GetStringAsync(apiUrl);
                _logger.LogInformation("Respuesta de la API: {response}", response);

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response);

                if (apiResponse?.Status?.Result == "Error")
                {
                    _logger.LogError("Error en la respuesta de la API: {ErrorCode} - {Message}",
                        apiResponse.Status.ErrorCode,
                        apiResponse.Status.Message);
                    return;
                }

                if (apiResponse?.Result == null)
                {
                    _logger.LogInformation("No se recibieron resultados de la API");
                    return;
                }

                int newPositionsCount = 0;
                foreach (var result in apiResponse.Result)
                {
                    // Verificar si este registro es más reciente que nuestro último registro
                    // y si está dentro de la ventana de tiempo de 3 minutos
                    if (!lastRegisteredTimes.TryGetValue(result.Uid, out var lastTime) ||
                        (result.LastReportedTimeUTC > lastTime &&
                         result.LastReportedTimeUTC > filterTime))
                    {
                        _logger.LogInformation("Guardando nueva posición para {uid} - {name}", result.Uid, result.Name);

                        var existingTracking = await _context.VehicleTrackings
                            .Include(v => v.SensorReadings)
                            .Include(v => v.InputOutputs)
                            .FirstOrDefaultAsync(v => v.Uid == result.Uid);

                        if (existingTracking == null)
                        {
                            existingTracking = new VehicleTracking
                            {
                                Uid = result.Uid,
                                Name = result.Name,
                                Imei = result.Imei,
                                LastReportedTimeLocal = result.LastReportedTimeLocal,
                                LastReportedTimeUTC = result.LastReportedTimeUTC,
                                SensorReadings = result.SensorReadings.Select(sr => new SensorReading
                                {
                                    UnitUid = sr.UnitUid,
                                    Name = sr.Name,
                                    Value = sr.Value,
                                    MeasurementSign = sr.MeasurementSign,
                                    ReadingTimeLocal = sr.ReadingTimeLocal,
                                    SensorType = sr.SensorType
                                }).ToList(),
                                InputOutputs = result.Position.InputOutputs.Select(io => new InputOutput
                                {
                                    SystemName = io.SystemName,
                                    Description = io.Description,
                                    UserDescription = io.UserDescription,
                                    Active = io.Active
                                }).ToList()
                            };

                            await _context.VehicleTrackings.AddAsync(existingTracking);
                            await _context.SaveChangesAsync(); // Save to get the ID
                        }
                        else
                        {
                            existingTracking.LastReportedTimeLocal = result.LastReportedTimeLocal;
                            existingTracking.LastReportedTimeUTC = result.LastReportedTimeUTC;

                            existingTracking.SensorReadings = result.SensorReadings.Select(sr => new SensorReading
                            {
                                UnitUid = sr.UnitUid,
                                Name = sr.Name,
                                Value = sr.Value,
                                MeasurementSign = sr.MeasurementSign,
                                ReadingTimeLocal = sr.ReadingTimeLocal,
                                SensorType = sr.SensorType
                            }).ToList();

                            existingTracking.InputOutputs = result.Position.InputOutputs.Select(io => new InputOutput
                            {
                                SystemName = io.SystemName,
                                Description = io.Description,
                                UserDescription = io.UserDescription,
                                Active = io.Active
                            }).ToList();

                            _context.VehicleTrackings.Update(existingTracking);
                        }

                        var newPosition = new Position
                        {
                            Latitude = result.Position.Latitude,
                            Longitude = result.Position.Longitude,
                            Address = result.Position.Address,
                            Speed = result.Position.Speed,
                            SpeedMeasure = result.Position.SpeedMeasure,
                            Heading = result.Position.Heading,
                            Ignition = result.Position.Ignition,
                            Odometer = result.Position.Odometer,
                            EngineTime = result.Position.EngineTime,
                            EngineStatus = result.Position.EngineStatus,
                            ServerTimeUTC = result.Position.ServerTimeUTC,
                            GPSTimeLocal = result.Position.GPSTimeLocal,
                            GPSTimeUtc = result.Position.GPSTimeUtc,
                            PointOfInterestUid = result.Position.PointOfInterest?.Uid,
                            PointOfInterestName = result.Position.PointOfInterest?.Name,
                            VehicleTrackingId = existingTracking.Id // Asociar la posición con la camioneta
                        };

                        await _context.Positions.AddAsync(newPosition);
                        newPositionsCount++;
                    }
                }

                if (newPositionsCount > 0)
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Se guardaron {count} nuevas posiciones", newPositionsCount);
                }
                else
                {
                    _logger.LogInformation("No se encontraron nuevas posiciones para guardar");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FetchAndSaveLatestPositions");
                throw;
            }
        }
    }

}