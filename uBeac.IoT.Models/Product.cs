using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public partial class Product
    {
        public Product()
        {
            Gateways = new HashSet<Gateway>();
        }

        public Guid Id { get; set; }
        public Guid ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Processor { get; set; }

        public virtual ICollection<Gateway> Gateways { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
