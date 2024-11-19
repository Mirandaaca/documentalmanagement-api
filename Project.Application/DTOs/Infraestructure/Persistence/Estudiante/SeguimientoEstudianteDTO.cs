using Project.Application.DTOs.Infraestructure.Persistence.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Estudiante
{
  public class SeguimientoEstudianteDTO
  {
    public int IdEstudiante { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string NroRegistro { get; set; }
    public List<DocumentoFaltanteDTO> DocumentosFaltantes { get; set; }
    public DateTime? FechaProximaExpiracion { get; set; } // Mantiene la fecha
    public int? DiasParaProximaExpiracion { get; set; }   // Agrega los días restantes
  }
}
