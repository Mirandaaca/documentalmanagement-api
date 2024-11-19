using Microsoft.AspNetCore.Identity;
using Project.Domain.Entities;
using Project.Identity;
using Project.Identity.Seeds;
using Project.Persistence;
using Project.WebApi.Extensions;

namespace Project.WebApi
{
  public class Startup
  {
    public IConfiguration _configuration { get; }

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddPersistenceInfraestructure(_configuration);
      services.AddIdentityInfraestructure(_configuration);
      services.AddControllers();

      services.AddEndpointsApiExplorer();

      services.AddSwaggerGen();

      //var originesPermitidos = _configuration.GetValue<string>("OrigenesPermitidos")!.Split(',');
      //services.AddCors(options =>
      //{
      //  options.AddDefaultPolicy(policy =>
      //  {
      //    policy.WithOrigins(originesPermitidos).AllowAnyHeader().AllowAnyMethod();
      //  });
      //});
      services.AddCors(options => {
        options.AddPolicy("NewPolicy",
                   builder =>
                   {
                     builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                   });
      });

    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseCors("NewPolicy");
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      // Configure the HTTP request pipeline.
      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      app.UseSwagger();
      app.UseSwaggerUI();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
      var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
      using (var scope = scopeFactory.CreateScope())
      {
        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<Usuario>>();

        DefaultRoles.SeedAsync(userManager, roleManager).Wait();
        DefaultAdminUser.SeedAsync(userManager, roleManager).Wait();
      }
      app.UseErrorHandlingMiddleware();
    }
  }
}
