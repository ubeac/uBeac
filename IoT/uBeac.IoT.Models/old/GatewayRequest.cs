using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class GatewayRequest
    {
        public string TraceId { get; set; }
        public string Body { get; set; }
        public Guid GatewayId { get; set; }
        public DateTime DateTime { get; set; }
        public string Url { get; set; }
        public List<DeviceRawData> RawDevices { get; set; }
        public List<DeviceData> Devices { get; set; }
        public byte[] Bytes { get; set; }
        public List<string> Exceptions { get; set; }
        public string RequestProtocol { get; set; }
        public string RequestMethod { get; set; }
    }
}
