using System;

namespace uBeac.IoT.Models
{
    public class Gateway : Device
    {
        public Guid FirmwareId { get; set; }
        public GatewaySecurity Security { get; set; }
    }
}