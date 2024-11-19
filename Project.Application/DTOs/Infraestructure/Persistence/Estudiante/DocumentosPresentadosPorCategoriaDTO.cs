using Project.Application.DTOs.Infraestructure.Persistence.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTOs.Infraestructure.Persistence.Estudiante
{
  public class DocumentosPresentadosPorCategoriaDTO
  {
    public string Categoria { get; set; }
    public List<ReadDocumentoPresentadoDTO> DocumentosPresentados { get; set; }
  }
}
