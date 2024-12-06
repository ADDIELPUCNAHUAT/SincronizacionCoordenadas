using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SincronizacionCoordenadas.Entity.dto
{
    public class ApiResponse
    {
        public Status? Status { get; set; }
        public List<ApiResult>? Result { get; set; }
    }

    public class Status
    {
        public string? Result { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
    }

    public class ApiResult
    {
        public string? Uid { get; set; }
        public string? Name { get; set; }
        public string? Imei { get; set; }
        public DateTime LastReportedTimeLocal { get; set; }
        public DateTime LastReportedTimeUTC { get; set; }
        public ApiPosition? Position { get; set; }
        public List<ApiSensorReading>? SensorReadings { get; set; }
    }

    public class ApiPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public double Speed { get; set; }
        public string? SpeedMeasure { get; set; }
        public double Heading { get; set; }
        public string? Ignition { get; set; }
        public double Odometer { get; set; }
        public double EngineTime { get; set; }
        public string? EngineStatus { get; set; }
        public DateTime ServerTimeUTC { get; set; }
        public DateTime GPSTimeLocal { get; set; }
        public DateTime GPSTimeUtc { get; set; }
        public PointOfInterest? PointOfInterest { get; set; }
        public List<InputOutput>? InputOutputs { get; set; }
    }

    public class PointOfInterest
    {
        public string? Uid { get; set; }
        public string? Name { get; set; }
    }

    public class ApiSensorReading
    {
        public string? UnitUid { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? MeasurementSign { get; set; }
        public DateTime ReadingTimeLocal { get; set; }
        public string? SensorType { get; set; }
    }
}