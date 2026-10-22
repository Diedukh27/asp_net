
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>Статус авто: В наявності, Заброньовано, Продано.</summary>
[Table("vehicle_statuses")]
[Index(nameof(Name), IsUnique = true)]
public class VehicleStatusEntity
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;
    //public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
