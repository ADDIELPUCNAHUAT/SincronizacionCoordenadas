using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizacionCoordenadas.Entity
{
    public class VehicleTracking
    {
        [Key]
        public int Id { get; set; }
        public string? Uid { get; set; }
        public string? Name { get; set; }
        public string? Imei { get; set; }
        public DateTime LastReportedTimeLocal { get; set; }
        public DateTime LastReportedTimeUTC { get; set; }

        public int PositionId { get; set; }
        public ICollection<Position> Positions { get; set; } = new List<Position>();


        public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();

        public ICollection<InputOutput> InputOutputs { get; set; } = new List<InputOutput>();
    }

    public class Position
    {
        [Key]
        public int Id { get; set; }
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
        public string? PointOfInterestUid { get; set; }
        public string? PointOfInterestName { get; set; }

        public int VehicleTrackingId { get; set; }
        public VehicleTracking? VehicleTracking { get; set; }
    }

    public class SensorReading
    {
        [Key]
        public int Id { get; set; }
        public string? UnitUid { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? MeasurementSign { get; set; }
        public DateTime ReadingTimeLocal { get; set; }
        public string? SensorType { get; set; }

        public int VehicleTrackingId { get; set; }
        public VehicleTracking? VehicleTracking { get; set; }
    }
    public class InputOutput
    {
        [Key]
        public int Id { get; set; }
        public string? SystemName { get; set; }
        public string? Description { get; set; }
        public string? UserDescription { get; set; }
        public bool Active { get; set; }

        public int VehicleTrackingId { get; set; }
        public VehicleTracking? VehicleTracking { get; set; }
    }
}
