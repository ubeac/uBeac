using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class Building : IEntity
    {
        public Building()
        {
            BuildingZones = new HashSet<BuildingZone>();
        }

        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<BuildingZone> BuildingZones { get; set; }
    }
}
