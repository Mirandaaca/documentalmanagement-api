using Project.WebApi.Middlewares;

namespace Project.WebApi.Extensions
{
  public static class UseErrorHandlerMiddleware
  {
    public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
      app.UseMiddleware<ErrorHandlerMiddleware>();
    }
  }
}
