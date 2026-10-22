using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>Марка: Volkswagen, Mercedes-Benz, MAN...</summary>
[Table("brands")]
[Index(nameof(Name), IsUnique = true)]
public class BrandEntity
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    //public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
