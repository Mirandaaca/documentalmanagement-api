using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Estudiante
{
  public class ReadEstudiantesConDocumentosFaltantesPorCategoriaDTO
  {
    public int IdEstudiante { get; set; }
    public string NombreCompleto { get; set; }
    public string Registro { get; set; }
    public string Email { get; set; }
    public List<DocumentosFaltantesPorCategoriaDTO> DocumentosFaltantes { get; set; }
  }
}
