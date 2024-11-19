using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Identity.Authentication
{
  public class AuthenticationRequest
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
