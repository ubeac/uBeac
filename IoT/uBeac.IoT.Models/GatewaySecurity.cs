namespace uBeac.IoT.Models
{
    public class GatewaySecurity
    {
        public GatewaySecurityHttp Http { get; set; }
        public GatewaySecurityMqtt Mqtt { get; set; }
        public GatewaySecurityIpRestriction IpRestriction { get; set; }
    }
}
