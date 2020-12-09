using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Edge : BaseEntity
    {
        public string Name { get; set; }
        public Guid FirmwareId { get; set; }
        public EdgeHttpSecurity Http { get; set; }
        public EdgeMqttSecurity Mqtt { get; set; }
        public EdgeIpRestriction IpRestriction { get; set; }
    }
  
    public class EdgeHttpSecurity
    {
        public bool SecureOnly { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
    public class EdgeMqttSecurity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SecureOnly { get; set; }
        public bool Enabled { get; set; }
    }
    public class EdgeIpRestriction
    {
        public List<string> AllowedIps { get; set; }
        public List<string> DeniedIps { get; set; }
    }
}