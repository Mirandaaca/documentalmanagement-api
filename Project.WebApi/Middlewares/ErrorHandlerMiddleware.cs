using Project.Application.Wrappers;
using System.Net;
using System.Text.Json;

namespace Project.WebApi.Middlewares
{
  public class ErrorHandlerMiddleware
  {
    private readonly RequestDelegate _next;
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception error)
      {
        var response = context.Response;
        response.ContentType = "application/json";
        var responseModel = new Response<string>() { Succeded = false, Message = error?.Message };

        switch (error)
        {
          case KeyNotFoundException e:
            //not found error
            response.StatusCode = (int)HttpStatusCode.NotFound;
            break;
          default:
            // unhandled error
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }
        var options = new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var result = JsonSerializer.Serialize(responseModel, options);

        await response.WriteAsync(result);
      }
    }
  }
}
