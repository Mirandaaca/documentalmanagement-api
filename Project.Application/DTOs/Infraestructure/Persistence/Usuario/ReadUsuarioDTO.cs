using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Usuario
{
  public class ReadUsuarioDTO
  {
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime FechaDeNacimiento { get; set; }
  }
}
