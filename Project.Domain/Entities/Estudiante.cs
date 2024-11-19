using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities
{
  public class Estudiante
  {
    [Key]
    public int Id { get; set; }
    public int IdCarrera { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string NroRegistro { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Estado { get; set; } = "Admision"; //Admision, Habilitacion, Egresado
    public string Telefono { get; set; }
    public string Email { get; set; }
    [ForeignKey("IdCarrera")]
    public Carrera Carrera { get; set; }
    //Relación de uno a muchos con Documento, un estudiante puede tener muchos documentos
    public List<Documento> Documentos { get; set; }
  }
}
