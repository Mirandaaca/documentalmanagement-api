using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project.Application.Interfaces.Identity;
using Project.Application.Wrappers;
using Project.Domain.Entities;
using Project.Identity.Services;
using Project.Persistence.Context;
namespace Project.Identity
{
    public static class ServiceExtensions
    {
      public static void AddIdentityInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddIdentity<Usuario, IdentityRole>()
                .AddEntityFrameworkStores<DocumentalManagement>()
            .AddDefaultTokenProviders();

      services.AddHttpContextAccessor();

      services.AddTransient<IUsuarioService, UsuarioService>();

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(o =>
      {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero,
          ValidIssuer = configuration["JWTSettings:Issuer"],
          ValidAudience = configuration["JWTSettings:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:SecretKey"]))
        };

        o.Events = new JwtBearerEvents()
        {
          OnAuthenticationFailed = context =>
          {
            context.Response.OnStarting(async () =>
            {
              context.Response.StatusCode = 401;
              context.Response.ContentType = "text/plain";
              await context.Response.WriteAsync("Error de autenticación");
            });
            return Task.CompletedTask;
          },
          OnChallenge = context =>
          {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response<string>("Usted no esta autorizado"));
            return context.Response.WriteAsync(result);
          },
          OnForbidden = context =>
          {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response<string>("Usted no tiene permisos sobre este recurso"));
            return context.Response.WriteAsync(result);
          }
        };
      });
      // Configuración de autorización de roles 
      services.AddAuthorization(options =>
      {
        options.AddPolicy("Administrador", policy =>
            policy.RequireRole("Administrador"));
        //usuario admisiones
        //graduados
        //haiblitaciones
      });
    }
    }
}
