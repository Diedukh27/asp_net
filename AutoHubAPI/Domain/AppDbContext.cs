using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<FuelTypeEntity> FuelTypes { get; set; }
    public DbSet<VehicleStatusEntity> VehicleStatuses { get; set; }
    public DbSet<LeadStatusEntity> LeadStatuses { get; set; }
    public DbSet<BodyTypeEntity> BodyTypes { get; set; }
    public DbSet<BrandEntity> Brands { get; set; }
}
