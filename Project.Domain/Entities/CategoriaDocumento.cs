using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities
{
  public class CategoriaDocumento
  {
    [Key]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    // Relación de uno a muchos con TipoDocumento
    public List<TipoDocumento> TiposDocumento { get; set; }
  }
}
