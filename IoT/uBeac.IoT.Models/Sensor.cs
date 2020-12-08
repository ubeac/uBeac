using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class Sensor : BaseTeamEntity
    {
        public Guid DeviceId { get; set; }
        public string Uid { get; set; }
        public int TypeId { get; set; }
        public int UnitId { get; set; }
        public int PrefixId { get; set; }
        public string Description { get; set; }
        public List<string> Schema { get; set; }
    }
}
