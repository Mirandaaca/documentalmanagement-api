using Project.Application.DTOs.Infraestructure.Identity.Authentication;
using Project.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Identity
{
  public interface IUsuarioService
  {
    public Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
  }
}
