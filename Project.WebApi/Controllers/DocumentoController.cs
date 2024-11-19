using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Infraestructure.Persistence.Documento;
using Project.Application.Interfaces.Persistence;
using Project.Persistence.Context;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DocumentoController : ControllerBase
  {
    private readonly IDocumentoRepository _documentoRepository;
    private readonly DocumentalManagement _context;
    private readonly ISendEmailService _sendEmailService;
    public DocumentoController(IDocumentoRepository documentoRepository, DocumentalManagement context, ISendEmailService sendEmailService)
    {
      _documentoRepository = documentoRepository;
      _context = context;
      _sendEmailService = sendEmailService;
    }
    [HttpPost("VerificarDocumentoExistente")]
    public async Task<IActionResult> VerificarDocumentoExistente([FromBody] VerificarDocumentoDTO dto)
    {
      try
      {
        var documentoExistente = await _context.Documentos
            .Include(d => d.Estudiante)
            .Include(d => d.Gestion)
            .FirstOrDefaultAsync(d =>
                d.IdGestion == dto.IdGestion &&
                d.IdEstudiante == dto.IdEstudiante &&
                d.IdTipoDocumento == dto.IdTipoDocumento);

        return Ok(new
        {
          existe = documentoExistente != null,
          documento = documentoExistente != null ? new
          {
            nombreEstudiante = documentoExistente.Estudiante.Nombre + " " + documentoExistente.Estudiante.Apellido,
            registroEstudiante = documentoExistente.Estudiante.NroRegistro,
            anioGestion = documentoExistente.Gestion.AnioGestion
          } : null
        });
      }
      catch (Exception ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }
  
    [HttpPost("SubirDocumento")]
    public async Task<IActionResult> SubirDocumento([FromForm] int IdEstudiante, [FromForm] int IdGestion, [FromForm] string IdUsuario, [FromForm] int IdTipoDocumento, IFormFile documento, [FromForm] bool legalizado)
    {
      if (documento == null || documento.Length == 0)
        return BadRequest("No se proporcionó un archivo válido.");

      try
      {
        var uploadCertificado = new UploadDocumentoDTO
        {
          IdEstudiante = IdEstudiante,
          IdGestion = IdGestion,
          IdUsuario = IdUsuario,
          IdTipoDocumento = IdTipoDocumento,
          Legalizado = legalizado,
        };

        await _documentoRepository.UploadDocumento(uploadCertificado, documento);
        return Ok("Certificado subido exitosamente.");
      }
      catch (Exception ex)
      {
        // Log the exception
        return StatusCode(500, "Ocurrió un error al subir el certificado.");
      }
    }
    [HttpGet("ObtenerDocumentos")]
    public async Task<IActionResult> ObtenerDocumentos()
    {
      var documentos = await _documentoRepository.ObtenerDocumentos();
      return Ok(documentos);
    }
    [HttpDelete("EliminarDocumento")]
    public async Task<IActionResult> EliminarDocumento(int id)
    {
      await _documentoRepository.EliminarDocumento(id);
      return Ok();
    }
    [HttpGet("ObtenerDocumentosPorEstudiante")]
    public async Task<IActionResult> ObtenerDocumentosPorEstudiante(int idEstudiante)
    {
      var documentos = await _documentoRepository.ObtenerDocumentosPorEstudiante(idEstudiante);
      return Ok(documentos);
    }
    [HttpGet("ObtenerDocumentosPorGestion")]
    public async Task<IActionResult> ObtenerDocumentosPorGestion(int idGestion)
    {
      var documentos = await _documentoRepository.ObtenerDocumentosPorGestion(idGestion);
      return Ok(documentos);
    }
    [HttpGet("ObtenerDocumentoPorId")]
    public async Task<IActionResult> ObtenerDocumentoPorId(int id)
    {
      var documento = await _documentoRepository.ObtenerDocumentoPorId(id);
      return Ok(documento);
    }
    [HttpGet("SeguimientoEstudiantes")]
    public async Task<IActionResult> ObtenerSeguimientoEstudiantes()
    {
      try
      {
        var seguimiento = await _documentoRepository.ObtenerSeguimientoEstudiantesAsync();
        return Ok(seguimiento);
      }
      catch (Exception ex)
      {
        return BadRequest(new { mensaje = ex.Message });
      }
    }
    [HttpPost("EnviarEmailATodos")]
    public async Task<IActionResult> EnviarEmailATodosAsync()
    {
      var estudiantesFaltantes = await _documentoRepository.ObtenerSeguimientoEstudiantesAsync();

      foreach (var estudiante in estudiantesFaltantes)
      {
        string mensaje = _sendEmailService.GenerarMensajeEmail(estudiante);
        await _sendEmailService.SendEmailAsync(estudiante.Correo, "Documentos Faltantes", mensaje);
      }
      return Ok(new { message = "Correos enviados exitosamente." });
    }
    [HttpPost("EnviarEmailIndividual")]
    public async Task<IActionResult> EnviarEmailIndividualAsync(string email)
    {
      var estudiantesFaltantes = await _documentoRepository.ObtenerSeguimientoEstudiantesAsync();
      var estudiante = estudiantesFaltantes.FirstOrDefault(e => e.Correo == email);

      if (estudiante == null)
      {
        return NotFound(new { message = "No se encontraron documentos faltantes para el correo proporcionado." });
      }

      string mensaje = _sendEmailService.GenerarMensajeEmail(estudiante);
      await _sendEmailService.SendEmailAsync(estudiante.Correo, "Documentos Faltantes", mensaje);
      return Ok(new { message = "Correo enviado exitosamente." });
    }
  }
}
