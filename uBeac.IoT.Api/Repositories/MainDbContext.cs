using Microsoft.EntityFrameworkCore;
using uBeac.IoT.Models;

namespace uBeac.IoT.Api.Repositories
{
    public partial class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }


        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<BuildingZone> BuildingZones { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<GatewayData> GatewayDatas { get; set; }
        public virtual DbSet<GatewayHttpHeader> GatewayHttpHeaders { get; set; }
        public virtual DbSet<GatewayIPRestriction> GatewayIPRestrictions { get; set; }
        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorSchema> SensorSchemas { get; set; }
        public virtual DbSet<SensorSchemaValue> SensorSchemaValues { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamToken> TeamTokens { get; set; }
        public virtual DbSet<TeamUser> TeamUsers { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
    }
}
