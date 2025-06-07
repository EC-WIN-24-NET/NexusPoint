using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.Data;
using Core.Interfaces.Factories;
using Domain;

namespace Core.Services;

public class LocationService(
    ILocationRepository locationRepository,
    IRepositoryResultFactory resultFactory,
    ILocationDtoFactory locationDtoFactory
) : ILocationService
{
    public async Task<RepositoryResult<LocationDisplay>> GetLocationByGuid(Guid id)
    {
        try
        {
            // Get the location from the repository
            var locationResult = await locationRepository.GetAsync(
                e => e != null && e.Id == id,
                false
            );

            // Case 1: Repository operation itself failed (e.g., DB connection issue, invalid query).
            // locationResult.Error will be a specific error type, not Error.NonError.
            if (locationResult.Error != Error.NonError)
            {
                return resultFactory.OperationFailed<LocationDisplay>(
                    locationResult.Error,
                    locationResult.StatusCode
                );
            }

            // Case 2: Entity was found.
            // BaseRepository.GetAsync returns Value != null and StatusCode = 200.
            // This means the location was successfully retrieved.
            if (locationResult.Value != null)
            {
                var displayLocationDto = locationDtoFactory.ToDisplay(locationResult.Value);
                return resultFactory.OperationSuccess(
                    displayLocationDto,
                    locationResult.StatusCode
                );
            }

            // Case 3: Entity was not found.
            if (locationResult.StatusCode == 404)
            {
                return resultFactory.OperationFailed<LocationDisplay>(
                    Error.NotFound("Location is not found"),
                    404
                );
            }

            // Case 4: Unexpected state after retrieving location details.
            return resultFactory.OperationFailed<LocationDisplay>(
                new Error(
                    "LocationService.UnexpectedState",
                    "An unexpected state was encountered after retrieving location details."
                ),
                500 //
            );
        }
        catch (Exception)
        {
            // Log the exception ex (recommended)
            return resultFactory.OperationFailed<LocationDisplay>(
                new Error(
                    "location.RetrievalError",
                    "An unexpected error occurred while retrieving user details."
                ),
                500 // Internal Server Error
            );
        }
    }
}
