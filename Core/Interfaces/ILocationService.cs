using Core.DTOs;
using Domain;

namespace Core.Interfaces;

public interface ILocationService
{

    Task<RepositoryResult<LocationDisplay>> GetLocationByGuid(Guid id);
}