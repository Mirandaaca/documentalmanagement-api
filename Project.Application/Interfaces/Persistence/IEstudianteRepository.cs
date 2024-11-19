using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface IEstudianteRepository
  {
    public Task GuardarEstudiante(CreateEstudianteDTO estudiante);
    public Task<List<ReadEstudianteDTO>> ObtenerEstudiantes();
    public Task<ReadEstudianteDTO> ObtenerEstudiantePorId(int id);
    public Task ActualizarEstudiante(UpdateEstudianteDTO estudiante);
    public Task EliminarEstudiante(int id);
    public Task<List<ReadEstudiantesConDocumentosPresentadosPorCategoriaDTO>> ObtenerEstudiantesConDocumentosPresentadosPorCategoria();
    public Task<List<ReadEstudiantesConDocumentosFaltantesYPresentadosDTO>> ObtenerEstudiantesConDocumentosFaltantesYPresentados();
    public Task<List<ReadEstudiantesConDocumentosFaltantesPorCategoriaDTO>> ObtenerEstudiantesConDocumentosFaltantesPorCategoria();
  }
}
