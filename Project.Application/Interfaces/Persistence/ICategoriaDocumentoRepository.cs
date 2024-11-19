using Project.Application.DTOs.Infraestructure.Persistence.CategoriaDocumento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface ICategoriaDocumentoRepository
  {
    public Task GuardarCategoriaDocumento(CreateCategoriaDocumentoDTO categoriaDocumento);
    public Task<List<ReadCategoriaDocumentoDTO>> ObtenerCategoriasDocumento();
    public Task<ReadCategoriaDocumentoDTO> ObtenerCategoriaDocumentoPorId(int id);
    public Task ActualizarCategoriaDocumento(UpdateCategoriaDocumentoDTO categoriaDocumento);
    public Task EliminarCategoriaDocumento(int id);
  }
}
