namespace uBeac.IoT.Models
{
    public class GatewaySecurityMqtt
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SecureOnly { get; set; }
        public bool Enabled { get; set; }
    }
}
