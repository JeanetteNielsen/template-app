using TemplateApp.Shared.Exceptions;

namespace TemplateApp.Server.Infrastructure
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var error = CreateErrorResponse(ex);
                context.Response.StatusCode = error.ErrorStatusCodes;

                await context.Response.WriteAsync(error.Message);
            }
        }

        private ErrorResponse CreateErrorResponse(Exception e)
        {
            int statusCode;
            string msg = "Unexpected error occured";

            switch (e)
            {
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    logger.LogInformation($"UnauthorizedAccessException: {e}", e);
                    break;
                case EntityNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    msg = "Entity not found";
                    // These are usually fine in multiuser systems.
                    // But it need to monitored if we see a spike we might have an broken integration to UI or other systems.
                    // Setup a threshold alert in sentry
                    logger.LogWarning($"EntityNotFoundException: {e}", e);
                    break;

                case CustomValidationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    msg = e.Message;
                    logger.LogInformation($"ValidationException: {e}", e);
                    break;
                default:
                    logger.LogError($"An unexpected error occurred: {e}", e);
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            return new ErrorResponse(statusCode, msg);
        }

        public record ErrorResponse(int ErrorStatusCodes, string Message);
    }
}