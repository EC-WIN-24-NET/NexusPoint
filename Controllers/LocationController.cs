using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NexusPoint.Helpers;
using NexusPoint.Interface;

namespace NexusPoint.Controllers;

/// <summary>
/// LocationController
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LocationController(
    ILocationService locationService,
    IWebHostEnvironment webHostEnvironment
) : ControllerBase
{
    /// <summary>
    /// Get Location by Guid
    /// /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpGet("{guid:Guid}", Name = "GetImageByGuid")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetLocationByGuid([FromRoute] Guid guid)
    {
        try
        {
            // Validate the Guid
            if (guid == Guid.Empty)
                return ApiResponseHelper.BadRequest("Invalid Guid provided.");

            // Get the status from the database
            var status = await locationService.GetLocationByGuid(guid);
            // Return the status
            return ApiResponseHelper.Success(status);
        }
        catch (Exception ex)
        {
            // Return a problem response, in development mode, it will include the stack trace
            return ApiResponseHelper.Problem(ex, webHostEnvironment.IsDevelopment());
        }
    }
}
