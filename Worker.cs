using SincronizacionCoordenadas.Service;

namespace SincronizacionCoordenadas
{
   public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var trackingService = scope.ServiceProvider.GetRequiredService<TrackingService>();
                await trackingService.FetchAndSaveLatestPositions();
            }

            await Task.Delay(40000, stoppingToken); // 2 minutos
        }
    }
}
}
