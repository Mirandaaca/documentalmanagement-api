using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Application.Interfaces.Persistence;
using Project.Persistence.Context;
using Project.Persistence.Repository;
using Project.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence
{
    public static class ServiceExtensions
    {
    public static void AddPersistenceInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<DocumentalManagement>(options => options.UseSqlServer(
          configuration.GetConnectionString("DefaultConnection"))
      );
      services.AddTransient<IFacultadRepository, FacultadRepository>();
      services.AddTransient<IGestionRepository, GestionRepository>();
      services.AddTransient<ICarreraRepository, CarreraRepository>();
      services.AddTransient<IEstudianteRepository, EstudianteRepository>();
      services.AddTransient<ICategoriaDocumentoRepository, CategoriaDocumentoRepository>();
      services.AddTransient<IDocumentoRepository, DocumentoRepository>();
      services.AddTransient<ITipoDocumentoRepository, TipoDocumentoRepository>();
      services.AddTransient<IUsuarioRepository, UsuarioRepository>();
      services.AddTransient<ISendEmailService, SendEmailService>();
    }
  }
}
