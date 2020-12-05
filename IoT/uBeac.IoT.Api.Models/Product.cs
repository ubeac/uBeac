using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class Product : IEntity
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
