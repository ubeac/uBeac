using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Manufacturer : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public Guid Logo { get; set; }
        public List<Product> Products { get; set; }
    }
}