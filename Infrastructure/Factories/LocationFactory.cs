using Domain;
using Infrastructure.Entities;

namespace Infrastructure.Factories;

/// <summary>
/// Event Factory
/// </summary>
public class LocationFactory : EntityFactoryBase<Location, LocationEntity>
{
    /// <summary>
    /// Creating from Entity object to Domain object
    /// Entity -> Domain
    /// </summary>
    /// <param name="locationEntity"></param>
    /// <returns></returns>
    public override Location ToDomain(LocationEntity locationEntity)
    {
        return new Location
        {
            Id = locationEntity.Id,
            StreetName = locationEntity.StreetName,
            City = locationEntity.City,
            State = locationEntity.State,
        };
    }

    /// <summary>
    /// Creating from Domain object to Entity object
    /// Domain -> Entity
    /// </summary>
    /// <param name="locations"></param>
    /// <returns></returns>
    public override LocationEntity ToEntity(Location locations)
    {
        return new LocationEntity
        {
            Id = locations.Id,
            StreetName = locations.StreetName,
            City = locations.City,
            State = locations.State,
        };
    }
}
