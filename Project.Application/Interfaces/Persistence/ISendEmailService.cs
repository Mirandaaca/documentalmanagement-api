using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface ISendEmailService
  {
    public Task SendEmailAsync(string email, string subject, string htmlMessage);
    public string GenerarMensajeEmail(SeguimientoEstudianteDTO estudiante);
  }
}
