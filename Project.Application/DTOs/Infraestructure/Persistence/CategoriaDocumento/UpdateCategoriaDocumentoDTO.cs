using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.CategoriaDocumento
{
  public class UpdateCategoriaDocumentoDTO
  {
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
  }
}
