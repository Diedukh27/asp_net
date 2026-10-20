using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket.Api.Data;

// ───────────────────────── Довідники ─────────────────────────

/// <summary>Тип палива: Дизель, Бензин, Газ, Газ/бензин, Гібрид, Електро.</summary>
[Table("fuel_types")]
[Index(nameof(Name), IsUnique = true)]
public class FuelType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

/// <summary>Статус авто: В наявності, Заброньовано, Продано.</summary>
[Table("vehicle_statuses")]
[Index(nameof(Name), IsUnique = true)]
public class VehicleStatus
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

/// <summary>Статус заявки: Нова, В роботі, Виконана, Відхилена.</summary>
[Table("lead_statuses")]
[Index(nameof(Name), IsUnique = true)]
public class LeadStatus
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
}

/// <summary>Тип кузова: Бортовий, Рефрижератор, Тент, Фургон, Дубль-кабіна борт, Самоскид, Евакуатор, Платформа.</summary>
[Table("body_types")]
[Index(nameof(Slug), IsUnique = true)]
public class BodyType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>URL-частина для фільтра/меню, напр. "dubl-kabina-bort".</summary>
    [Required, MaxLength(100)]
    public string Slug { get; set; } = null!;

    /// <summary>Картинка плитки у фільтрі.</summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public int SortOrder { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

/// <summary>Марка: Volkswagen, Mercedes-Benz, MAN...</summary>
[Table("brands")]
[Index(nameof(Name), IsUnique = true)]
public class Brand
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

// ───────────────────────── Автомобіль ─────────────────────────

[Table("vehicles")]
[Index(nameof(BrandId))]
[Index(nameof(BodyTypeId))]
[Index(nameof(Price))]
[Index(nameof(Year))]
[Index(nameof(FuelTypeId))]
[Index(nameof(StatusId), nameof(CreatedAt))]
public class Vehicle
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Brand))]
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    [ForeignKey(nameof(BodyType))]
    public int BodyTypeId { get; set; }
    public BodyType BodyType { get; set; } = null!;

    /// <summary>Модель: Crafter, Sprinter...</summary>
    [Required, MaxLength(100)]
    public string Model { get; set; } = null!;

    /// <summary>Підзаголовок картки, напр. "Комерційний автомобіль".</summary>
    [MaxLength(150)]
    public string? Subtitle { get; set; }

    [Range(1980, 2100)]
    public int Year { get; set; }

    /// <summary>Пробіг, км.</summary>
    [Range(0, 5_000_000)]
    public int Mileage { get; set; }

    /// <summary>Об'єм двигуна, л (напр. 2.0).</summary>
    [Column(TypeName = "numeric(3,1)")]
    [Range(0.1, 20)]
    public decimal EngineVolume { get; set; }

    [ForeignKey(nameof(FuelType))]
    public int FuelTypeId { get; set; }
    public FuelType FuelType { get; set; } = null!;

    /// <summary>Ціна в USD.</summary>
    [Column(TypeName = "numeric(12,2)")]
    [Range(0, 100_000_000)]
    public decimal Price { get; set; }

    [MaxLength(17)]
    public string? Vin { get; set; }

    public string? Description { get; set; }

    /// <summary>За замовчуванням 1 = "В наявності".</summary>
    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; } = 1;
    public VehicleStatus Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();
    public ICollection<VehicleSpecification> Specifications { get; set; } = new List<VehicleSpecification>();
    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
}

/// <summary>Фото авто (галерея на сторінці "Детальніше").</summary>
[Table("vehicle_images")]
[Index(nameof(VehicleId), nameof(SortOrder))]
public class VehicleImage
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Vehicle))]
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    [Required, MaxLength(500)]
    public string Url { get; set; } = null!;

    /// <summary>Головне фото для картки в каталозі.</summary>
    public bool IsMain { get; set; }

    public int SortOrder { get; set; }
}

/// <summary>Додаткові характеристики (коробка передач, потужність, колісна база тощо) у вигляді ключ-значення.</summary>
[Table("vehicle_specifications")]
[Index(nameof(VehicleId))]
public class VehicleSpecification
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Vehicle))]
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(300)]
    public string Value { get; set; } = null!;

    public int SortOrder { get; set; }
}

/// <summary>Заявка / запит на дзвінок з сайту (кнопка "Зателефонувати").</summary>
[Table("leads")]
[Index(nameof(StatusId), nameof(CreatedAt))]
public class Lead
{
    [Key]
    public int Id { get; set; }

    /// <summary>Авто, за яким залишили заявку (може бути null для загальної).</summary>
    [ForeignKey(nameof(Vehicle))]
    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(32), Phone]
    public string Phone { get; set; } = null!;

    [MaxLength(1000)]
    public string? Message { get; set; }

    /// <summary>За замовчуванням 1 = "Нова".</summary>
    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; } = 1;
    public LeadStatus Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// ───────────────────────── DbContext ─────────────────────────

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<BodyType> BodyTypes => Set<BodyType>();
    public DbSet<FuelType> FuelTypes => Set<FuelType>();
    public DbSet<VehicleStatus> VehicleStatuses => Set<VehicleStatus>();
    public DbSet<LeadStatus> LeadStatuses => Set<LeadStatus>();
    public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
    public DbSet<VehicleSpecification> VehicleSpecifications => Set<VehicleSpecification>();
    public DbSet<Lead> Leads => Set<Lead>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // не дозволяємо каскадом видаляти довідники, якщо є авто / заявки
        b.Entity<Vehicle>().HasOne(v => v.Brand).WithMany(x => x.Vehicles).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Vehicle>().HasOne(v => v.BodyType).WithMany(x => x.Vehicles).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Vehicle>().HasOne(v => v.FuelType).WithMany(x => x.Vehicles).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Vehicle>().HasOne(v => v.Status).WithMany(x => x.Vehicles).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Lead>().HasOne(l => l.Status).WithMany(x => x.Leads).OnDelete(DeleteBehavior.Restrict);

        // початкові дані довідників
        b.Entity<FuelType>().HasData(
            new FuelType { Id = 1, Name = "Дизель" },
            new FuelType { Id = 2, Name = "Бензин" },
            new FuelType { Id = 3, Name = "Газ" },
            new FuelType { Id = 4, Name = "Газ/бензин" },
            new FuelType { Id = 5, Name = "Гібрид" },
            new FuelType { Id = 6, Name = "Електро" }
        );

        b.Entity<VehicleStatus>().HasData(
            new VehicleStatus { Id = 1, Name = "В наявності" },
            new VehicleStatus { Id = 2, Name = "Заброньовано" },
            new VehicleStatus { Id = 3, Name = "Продано" }
        );

        b.Entity<LeadStatus>().HasData(
            new LeadStatus { Id = 1, Name = "Нова" },
            new LeadStatus { Id = 2, Name = "В роботі" },
            new LeadStatus { Id = 3, Name = "Виконана" },
            new LeadStatus { Id = 4, Name = "Відхилена" }
        );

        // заявка лишається, навіть якщо авто видалили
        b.Entity<Lead>().HasOne(l => l.Vehicle).WithMany(v => v.Leads).OnDelete(DeleteBehavior.SetNull);

        // початкові типи кузова зі сторінки каталогу
        b.Entity<BodyType>().HasData(
            new BodyType { Id = 1, Name = "Бортовий",         Slug = "bortovi",        SortOrder = 1 },
            new BodyType { Id = 2, Name = "Рефрижератор",     Slug = "refrigerators",  SortOrder = 2 },
            new BodyType { Id = 3, Name = "Тент",             Slug = "tent",           SortOrder = 3 },
            new BodyType { Id = 4, Name = "Фургон",           Slug = "furgon",         SortOrder = 4 },
            new BodyType { Id = 5, Name = "Дубль-кабіна борт",Slug = "dubl-kabina-bort", SortOrder = 5 },
            new BodyType { Id = 6, Name = "Самоскид",         Slug = "samoskyd",       SortOrder = 6 },
            new BodyType { Id = 7, Name = "Евакуатор",        Slug = "evakuator",      SortOrder = 7 },
            new BodyType { Id = 8, Name = "Платформа",        Slug = "platforma",      SortOrder = 8 }
        );
    }
}