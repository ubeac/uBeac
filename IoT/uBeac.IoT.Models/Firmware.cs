using System;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Firmware: BaseEntity
    {
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string TechnicalSpecFile { get; set; }
        public string Processor { get; set; }
    }
}