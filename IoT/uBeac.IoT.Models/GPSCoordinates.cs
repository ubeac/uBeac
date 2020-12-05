namespace uBeac.IoT.Models
{
    public class Coordinate
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Altitude { get; set; }
        public CoordinateType Type { get; set; }
    }    
}
