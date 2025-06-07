using Core.DTOs;
using Core.Interfaces.Factories;
using Domain;

namespace Core.Factories.DTO;

public class LocationDtoFactory : ILocationDtoFactory
{
    public LocationDisplay ToDisplay(Location displayLocation)
    {
        return new LocationDisplay
        {
            Id = displayLocation.Id,
            StreetName = displayLocation.StreetName,
            City = displayLocation.City,
            State = displayLocation.State,
        };
    }

    public Location ToDomain(LocationDisplay locationDisplay)
    {
        return new Location
        {
            Id = locationDisplay.Id,
            StreetName = locationDisplay.StreetName,
            City = locationDisplay.City,
            State = locationDisplay.State,
        };
    }
}
