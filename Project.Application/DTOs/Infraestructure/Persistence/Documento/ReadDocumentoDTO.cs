using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Documento
{
  public class ReadDocumentoDTO
  {
    public int Id { get; set; }
    public int IdGestion { get; set; }
    public int AnioGestion { get; set; }
    public int IdEstudiante { get; set; }
    public string NombreEstudiante { get; set; }
    public string RegistroEstudiante { get; set; }
    public string IdUsuario { get; set; }
    public int IdTipoDocumento { get; set; }
    public string NombreTipoDocumento { get; set; }
    public DateTime FechaSubida { get; set; }
    public byte[] Contenido { get; set; }
    public string Extension { get; set; }
    public string NombreArchivo { get; set; }
    public string Ruta { get; set; }
    public bool Legalizado { get; set; }
  }
}
