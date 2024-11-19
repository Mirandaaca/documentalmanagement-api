using Project.Application.DTOs.Infraestructure.Persistence.TipoDocumento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface ITipoDocumentoRepository
  {
    public Task GuardarTipoDocumento(CreateTipoDocumentoDTO tipoDocumento);
    public Task<List<ReadTipoDocumentoDTO>> ObtenerTiposDocumento();
    public Task<ReadTipoDocumentoDTO> ObtenerTipoDocumentoPorId(int id);
    public Task ActualizarTipoDocumento(UpdateTipoDocumentoDTO tipoDocumento);
    public Task EliminarTipoDocumento(int id);
  }
}
