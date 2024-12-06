using Microsoft.EntityFrameworkCore;
using SincronizacionCoordenadas.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizacionCoordenadas.Data.db
{
    public class TrackingContext : DbContext
    {
       public DbSet<VehicleTracking> VehicleTrackings { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
    public DbSet<InputOutput> InputOutputs { get; set; }

    public TrackingContext(DbContextOptions<TrackingContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones adicionales de relaciones si son necesarias
        modelBuilder.Entity<VehicleTracking>()
            .HasMany(v => v.Positions)
            .WithOne(p => p.VehicleTracking)
            .HasForeignKey(p => p.VehicleTrackingId);

        modelBuilder.Entity<VehicleTracking>()
            .HasMany(v => v.SensorReadings)
            .WithOne(s => s.VehicleTracking)
            .HasForeignKey(s => s.VehicleTrackingId);

        modelBuilder.Entity<VehicleTracking>()
            .HasMany(v => v.InputOutputs)
            .WithOne(io => io.VehicleTracking)
            .HasForeignKey(io => io.VehicleTrackingId);
    }
    }
}
