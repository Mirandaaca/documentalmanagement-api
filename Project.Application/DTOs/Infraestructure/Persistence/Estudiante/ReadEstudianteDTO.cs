using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Estudiante
{
  public class ReadEstudianteDTO
  {
    public int Id { get; set; }
    public int IdCarrera { get; set; }
    public string Carrera { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string NroRegistro { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Estado { get; set; } //Admision, Habilitacion, Egresado
    public string Telefono { get; set; }
    public string Email { get; set; }
  }
}
