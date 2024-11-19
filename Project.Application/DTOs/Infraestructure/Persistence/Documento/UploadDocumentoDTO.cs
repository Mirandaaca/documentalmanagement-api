using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Documento
{
  public class UploadDocumentoDTO
  {
    public int IdGestion { get; set; }
    public int IdEstudiante { get; set; }
    public string IdUsuario { get; set; }
    public int IdTipoDocumento { get; set; }
    public DateTime FechaSubida { get; set; }
    public string Extension { get; set; }
    public bool Legalizado { get; set; }
  }
}
