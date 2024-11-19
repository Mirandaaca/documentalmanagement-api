using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities
{
  public class Bitacora
  {
    [Key]
    public int Id { get; set; }
    public string IdUsuario { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public string Accion { get; set; }
    [ForeignKey("IdUsuario")]
    public Usuario Usuario { get; set; }
  }
}
