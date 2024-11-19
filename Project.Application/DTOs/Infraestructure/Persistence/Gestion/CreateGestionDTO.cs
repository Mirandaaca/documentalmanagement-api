using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Gestion
{
  public class CreateGestionDTO
  {
    public int AnioGestion { get; set; }
    public DateTime FechaPrimerSemestreInicio { get; set; }
    public DateTime FechaSegundoSemestreInicio { get; set; }
    public DateTime FechaPrimerSemestreFin { get; set; }
    public DateTime FechaSegundoSemestreFin { get; set; }
    public string CorreoInstitucion { get; set; }
  }
}
