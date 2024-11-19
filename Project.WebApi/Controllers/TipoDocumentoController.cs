using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Persistence.TipoDocumento;
using Project.Application.Interfaces.Persistence;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TipoDocumentoController : ControllerBase
  {
    private readonly ITipoDocumentoRepository _tipoDocumentoRepository;
    public TipoDocumentoController(ITipoDocumentoRepository tipoDocumentoRepository)
    {
      _tipoDocumentoRepository = tipoDocumentoRepository;
    }
    [HttpPost("GuardarTipoDocumento")]
    public async Task<IActionResult> CrearTipoDocumento([FromBody] CreateTipoDocumentoDTO tipoDocumento)
    {
      await _tipoDocumentoRepository.GuardarTipoDocumento(tipoDocumento);
      return Ok();
    }
    [HttpGet("ObtenerTipoDocumentos")]
    public async Task<IActionResult> ObtenerTipoDocumentos()
    {
      var tipoDocumentos = await _tipoDocumentoRepository.ObtenerTiposDocumento();
      return Ok(tipoDocumentos);
    }
    [HttpGet("ObtenerTipoDocumentoPorId")]
    public async Task<IActionResult> ObtenerTipoDocumentoPorId(int id)
    {
      var tipoDocumento = await _tipoDocumentoRepository.ObtenerTipoDocumentoPorId(id);
      return Ok(tipoDocumento);
    }
    [HttpPut("ActualizarTipoDocumento")]
    public async Task<IActionResult> ActualizarTipoDocumento([FromBody] UpdateTipoDocumentoDTO tipoDocumento)
    {
      await _tipoDocumentoRepository.ActualizarTipoDocumento(tipoDocumento);
      return Ok();
    }
    [HttpDelete("EliminarTipoDocumento")]
    public async Task<IActionResult> EliminarTipoDocumento(int id)
    {
      await _tipoDocumentoRepository.EliminarTipoDocumento(id);
      return Ok();
    }
  }
}
