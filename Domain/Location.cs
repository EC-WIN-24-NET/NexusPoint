namespace Domain;

public class Location
{
    public Guid Id { get; init; }
    public string StreetName { get; init; } = null!;
    public string City { get; init; } = null!;
    public string State { get; init; } = null!;
}
