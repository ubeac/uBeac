namespace uBeac.IoT.Models
{
    public class SensorType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MeasurementUnit Unit { get; set; }
        public MeasurementUnitPrefix Prefix { get; set; }
    }
    // todo: move this to db
    public enum SensorType1
    {
        NA = 0,
        Custom = 1,
        Location = 2,
        Range = 3,
        Temperature = 4,
        Humidity = 5,
        Voltage = 6,
        Acceleration = 7,
        MagneticField = 8,
        RotationalMotion = 9,
        Proximity = 10,
        Illuminance = 11,
        Pressure = 12,
        Counter = 13,
        Orientation = 14,
        Gyroscope = 15,
        Sound = 16,
        SignalStrength = 17,
        Processor = 18,
        Memory = 19,
        DiskSpace = 20,
        Bandwidth = 21
    }
}