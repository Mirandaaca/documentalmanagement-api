using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities
{
  public class Documento
  {
    [Key]
    public int Id { get; set; }
    public int IdGestion { get; set; }
    public int IdEstudiante { get; set; }
    public string IdUsuario { get; set; }
    public int IdTipoDocumento { get; set; }
    public DateTime FechaSubida { get; set; }
    public byte[] Contenido{ get; set; }
    public string Extension { get; set; }
    public string NombreArchivo { get; set; }
    public string Ruta { get; set; } = "localhost//5000/documentos";
    public bool Legalizado { get; set; } = true;
    [ForeignKey("IdGestion")]
    public Gestion Gestion { get; set; }
    [ForeignKey("IdEstudiante")]
    public Estudiante Estudiante { get; set; }
    [ForeignKey("IdUsuario")]
    public Usuario Usuario { get; set; }
    [ForeignKey("IdTipoDocumento")]
    public TipoDocumento TipoDocumento { get; set; }
  }
}
