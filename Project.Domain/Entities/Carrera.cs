using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities
{
  public class Carrera
  {
    [Key]
    public int Id { get; set; }
    public int IdFacultad { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    // Relación de muchos a uno con Facultad, una carrera pertenece a una facultad
    [ForeignKey("IdFacultad")]
    public Facultad Facultad { get; set; }
  }
}
