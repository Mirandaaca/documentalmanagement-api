using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Documento
{
  public class ReadDocumentoPresentadoDTO
  {
    public int IdDocumento { get; set; }
    public string NombreArchivo { get; set; }
    public DateTime FechaSubida { get; set; }
    public string TipoDocumentoNombre { get; set; }
  }
}
