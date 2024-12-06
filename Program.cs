using Microsoft.EntityFrameworkCore;
using SincronizacionCoordenadas;
using SincronizacionCoordenadas.Data.db;
using SincronizacionCoordenadas.Service;

    var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddDbContext<TrackingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<TrackingService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

await app.RunAsync();