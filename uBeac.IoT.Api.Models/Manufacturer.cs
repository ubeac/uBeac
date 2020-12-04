using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class Manufacturer : IEntity
    {
        public Manufacturer()
        {
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
