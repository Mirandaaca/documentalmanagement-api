using Project.Application.DTOs.Infraestructure.Persistence.Carrera;
using Project.Application.DTOs.Infraestructure.Persistence.Facultad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface ICarreraRepository
  {
    public Task GuardarCarrera(CreateCarreraDTO carrera);
    public Task<List<ReadCarreraDTO>> ObtenerCarreras();
    public Task<ReadCarreraDTO> ObtenerCarreraPorId(int id);
    public Task ActualizarCarrera(UpdateCarreraDTO facultad);
    public Task EliminarCarrera(int id);
  }
}
