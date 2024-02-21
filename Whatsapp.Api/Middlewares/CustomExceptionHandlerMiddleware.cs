using System.Diagnostics;
using Whatsapp.Api.AppException;
using Whatsapp.Api.AppResponse;
namespace Whatsapp.Api.Middlewares;

public class CustomExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

	public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (BadRequestException ex)
		{
			context.Response.StatusCode = 400;
			await context.Response.WriteAsJsonAsync(new BadRequestResponse(ex.Message, ex.Errors));
		}
		catch (Exception ex)
		{
			// Set the response status code
			context.Response.StatusCode = 500;

			// Write the error message to the response

			_logger.LogError(ex, "Could not process a request on machine {Machine}. TraceId: {TraceId}",
									Environment.MachineName,
									Activity.Current?.TraceId.ToString());

			await context.Response.WriteAsJsonAsync(new InternalServerErrorResponse(Activity.Current!.TraceId.ToString(), ex.Message));
		}
	}
}