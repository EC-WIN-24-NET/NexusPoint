using Core.Interfaces.Data;
using Core.Interfaces.Factories;
using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repository;

public class LocationRepository(
    DataContext dataContext,
    IEntityFactory<Domain.Location?, LocationEntity> factory,
    IRepositoryResultFactory resultFactory
) : BaseRepository<Domain.Location, LocationEntity>(dataContext, factory, resultFactory), ILocationRepository
{
    // If we want to override the methods from the BaseRepository
    // using override keyword, remember that the method needs to be virtual in the BaseRepository
}
