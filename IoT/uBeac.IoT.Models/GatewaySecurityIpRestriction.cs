using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class GatewaySecurityIpRestriction
    {
        public List<string> AllowedIps { get; set; }
        public List<string> DeniedIps { get; set; }
    }
}
