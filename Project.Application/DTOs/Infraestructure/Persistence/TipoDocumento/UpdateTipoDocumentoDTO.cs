using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.TipoDocumento
{
  public class UpdateTipoDocumentoDTO
  {
    public int Id { get; set; }
    public int IdCategoriaDocumento { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaExpiracion { get; set; }
  }
}
