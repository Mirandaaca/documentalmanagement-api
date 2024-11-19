using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Documento
{
  public class DocumentoFaltanteDTO
  {
    public string NombreDocumento { get; set; }
    public int DiasRestantes { get; set; }
    public bool EstaExpirado { get; set; }
    public string Semestre { get; set; } // Primer Semestre o Segundo Semestre
  }
}
