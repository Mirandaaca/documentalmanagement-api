using Project.Application.DTOs.Infraestructure.Persistence.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface IUsuarioRepository
  {
    public Task<ReadUsuarioRegistrationInformationDTO> GuardarUsuario(CreateUsuarioDTO usuario);
    public Task<List<ReadUsuarioDTO>> ObtenerUsuarios();
    public Task<ReadUsuarioDTO> ObtenerUsuarioPorId(string id);
    public Task ActualizarUsuario(UpdateUsuarioDTO usuario);
    public Task EliminarUsuario(string id);
  }
}
