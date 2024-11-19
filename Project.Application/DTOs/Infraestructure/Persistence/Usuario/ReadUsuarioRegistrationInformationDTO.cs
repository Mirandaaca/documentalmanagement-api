using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Usuario
{
  public class ReadUsuarioRegistrationInformationDTO
  {
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
  }
}
