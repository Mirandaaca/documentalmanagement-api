using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Documento
{
  public class VerificarDocumentoDTO
  {
    public int IdEstudiante { get; set; }
    public int IdGestion { get; set; }
    public int IdTipoDocumento { get; set; }
  }
}
