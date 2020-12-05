using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public Guid ManufacturerId { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }
        public Guid ManualFile { get; set; }
        public Guid TechnicalSpecFile { get; set; }
        public Guid Image { get; set; }
        public string Url { get; set; }
        public List<Firmware> Firmwares { get; set; }
    }
}