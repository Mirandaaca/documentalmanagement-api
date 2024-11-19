using Microsoft.AspNetCore.Http;
using Project.Application.DTOs.Infraestructure.Persistence.Documento;
using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Interfaces.Persistence
{
  public interface IDocumentoRepository
  {
    public Task UploadDocumento( UploadDocumentoDTO uploadCertificado, IFormFile documento);
    public Task<List<ReadDocumentoDTO>> ObtenerDocumentos();
    public Task<ReadDocumentoDTO> ObtenerDocumentoPorId(int id);
    //public Task ActualizarDocumento(UpdateDocumentoDTO documento);
    public Task<List<ReadDocumentoDTO>> ObtenerDocumentosPorEstudiante(int idEstudiante);
    public Task<List<ReadDocumentoDTO>> ObtenerDocumentosPorGestion(int idGestion);
    public Task<ReadDocumentoDTO> ObtenerDocumentoDeEstudiantePorId(int idEstudiante, int idDocumento);
    public Task EliminarDocumento(int id);
    public Task<List<SeguimientoEstudianteDTO>> ObtenerSeguimientoEstudiantesAsync();
  }
}
