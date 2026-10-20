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
}
