using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Persistence.CategoriaDocumento;
using Project.Application.Interfaces.Persistence;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriaDocumentoController : ControllerBase
  {
    private readonly ICategoriaDocumentoRepository _categoriaDocumentoRepository;
    public CategoriaDocumentoController(ICategoriaDocumentoRepository categoriaDocumentoRepository)
    {
      _categoriaDocumentoRepository = categoriaDocumentoRepository;
    }
    [HttpPost("GuardarCategoriaDocumento")]
    public async Task<IActionResult> CrearCategoriaDocumento([FromBody] CreateCategoriaDocumentoDTO categoriaDocumento)
    {
      await _categoriaDocumentoRepository.GuardarCategoriaDocumento(categoriaDocumento);
      return Ok();
    }
    [HttpGet("ObtenerCategoriaDocumentos")]
    public async Task<IActionResult> ObtenerCategoriaDocumentos()
    {
      var categoriaDocumentos = await _categoriaDocumentoRepository.ObtenerCategoriasDocumento();
      return Ok(categoriaDocumentos);
    }
    [HttpGet("ObtenerCategoriaDocumentoPorId")]
    public async Task<IActionResult> ObtenerCategoriaDocumentoPorId(int id)
    {
      var categoriaDocumento = await _categoriaDocumentoRepository.ObtenerCategoriaDocumentoPorId(id);
      return Ok(categoriaDocumento);
    }
    [HttpPut("ActualizarCategoriaDocumento")]
    public async Task<IActionResult> ActualizarCategoriaDocumento([FromBody] UpdateCategoriaDocumentoDTO categoriaDocumento)
    {
      await _categoriaDocumentoRepository.ActualizarCategoriaDocumento(categoriaDocumento);
      return Ok();
    }
    [HttpDelete("EliminarCategoriaDocumento")]
    public async Task<IActionResult> EliminarCategoriaDocumento(int id)
    {
      await _categoriaDocumentoRepository.EliminarCategoriaDocumento(id);
      return Ok();
    }
  }
}
