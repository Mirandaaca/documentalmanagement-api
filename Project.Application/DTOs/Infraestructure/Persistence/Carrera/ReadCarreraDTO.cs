using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Carrera
{
  public class ReadCarreraDTO
  {
    public int Id { get; set; }
    public int IdFacultad { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
  }
}
