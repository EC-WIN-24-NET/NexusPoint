using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class LocationEntity
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string StreetName { get; init; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string City { get; init; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string State { get; init; } = null!;
}
