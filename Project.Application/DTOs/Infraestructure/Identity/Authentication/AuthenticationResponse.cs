using Project.Application.DTOs.Infraestructure.Persistence.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Identity.Authentication
{
  public class AuthenticationResponse
  {
    public string IdUsuario { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string JWTToken { get; set; }
    public string Rol { get; set; }
    public ReadUserInformationDTO DatosUsuario { get; set; }
  }
}
