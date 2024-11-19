using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
using Project.Application.DTOs.Infraestructure.Persistence.Facultad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface IFacultadRepository
  {
    public Task GuardarFacultad(CreateFacultadDTO facultad);
    public Task<List<ReadFacultadDTO>> ObtenerFacultades();
    public Task<ReadFacultadDTO> ObtenerFacultadPorId(int id);
    public Task ActualizarFacultad(UpdateFacultadDTO facultad);
    public Task EliminarFacultad(int id);
  }
}
