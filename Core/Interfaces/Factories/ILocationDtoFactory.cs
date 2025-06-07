using Core.DTOs;
using Domain;

namespace Core.Interfaces.Factories;

public interface ILocationDtoFactory
{
    LocationDisplay ToDisplay(Location displayLocation);
    Location ToDomain(LocationDisplay locationDisplay);
}
