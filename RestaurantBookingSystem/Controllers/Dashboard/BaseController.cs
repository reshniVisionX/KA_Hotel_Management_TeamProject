using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;

namespace RestaurantBookingSystem.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected IActionResult HandleResponse<T>(T data, string successMessage = "Success")
        {
            try
            {
                if (data == null)
                {
                    _logger.LogWarning("Data not found");
                    return Ok(ApiResponse<T>.ErrorResponse("Data not found"));
                }

                _logger.LogInformation(successMessage);
                return Ok(ApiResponse<T>.SuccessResponse(data, successMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                return Ok(ApiResponse<T>.ErrorResponse("An error occurred while processing the request"));
            }
        }

        protected IActionResult HandleResponse(bool success, string successMessage, string errorMessage)
        {
            try
            {
                if (success)
                {
                    _logger.LogInformation(successMessage);
                    return Ok(ApiResponse.Success(successMessage));
                }

                _logger.LogWarning(errorMessage);
                return Ok(ApiResponse.Error(errorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                return Ok(ApiResponse.Error("An error occurred while processing the request"));
            }
        }

        protected async Task<IActionResult> ExecuteAsync<T>(Func<Task<T>> operation, string successMessage = "Operation completed successfully")
        {
            try
            {
                var result = await operation();
                return HandleResponse(result, successMessage);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument provided");
                return Ok(ApiResponse<T>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                return Ok(ApiResponse<T>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                return Ok(ApiResponse<T>.ErrorResponse("An unexpected error occurred"));
            }
        }

        protected async Task<IActionResult> ExecuteAsync(Func<Task<bool>> operation, string successMessage, string errorMessage)
        {
            try
            {
                var result = await operation();
                return HandleResponse(result, successMessage, errorMessage);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument provided");
                return Ok(ApiResponse.Error(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                return Ok(ApiResponse.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                return Ok(ApiResponse.Error("An unexpected error occurred"));
            }
        }
    }
}