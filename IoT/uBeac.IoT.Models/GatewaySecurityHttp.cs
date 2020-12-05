using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class GatewaySecurityHttp
    {
        public bool SecureOnly { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
