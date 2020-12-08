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
    public class Product
    {
        public Guid Id { get; set; }
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
    public class Firmware
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public string Processor { get; set; }
    }
}