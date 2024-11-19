using Project.Application.DTOs.Infraestructure.Persistence.Gestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface IGestionRepository
  {
    public Task GuardarGestion(CreateGestionDTO gestion);
    public Task<List<ReadGestionDTO>> ObtenerGestiones();
    public Task<ReadGestionDTO> ObtenerGestionPorId(int id);
    public Task ActualizarGestion(UpdateGestionDTO gestion);
    public Task EliminarGestion(int id);
  }
}
