using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class LocationDisplay
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public string StreetName { get; init; } = null!;

    [Required]
    public string City { get; init; } = null!;

    [Required]
    public string State { get; init; } = null!;
}
