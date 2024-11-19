using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Persistence.Gestion;
using Project.Application.Interfaces.Persistence;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GestionController : ControllerBase
  {
    private readonly IGestionRepository _gestionRepository;
    public GestionController(IGestionRepository gestionRepository)
    {
      _gestionRepository = gestionRepository;
    }
    [HttpPost("GaurdarGestion")]
    public async Task<IActionResult> CrearGestion([FromBody] CreateGestionDTO gestion)
    {
      await _gestionRepository.GuardarGestion(gestion);
      return Ok();
    }
    [HttpGet("ObtenerGestiones")]
    public async Task<IActionResult> ObtenerGestiones()
    {
      var gestiones = await _gestionRepository.ObtenerGestiones();
      return Ok(gestiones);
    }
    [HttpGet("ObtenerGestionPorId")]
    public async Task<IActionResult> ObtenerGestionPorId(int id)
    {
      var gestion = await _gestionRepository.ObtenerGestionPorId(id);
      return Ok(gestion);
    }
    [HttpPut("ActualizarGestion")]
    public async Task<IActionResult> ActualizarGestion([FromBody] UpdateGestionDTO gestion)
    {
      await _gestionRepository.ActualizarGestion(gestion);
      return Ok();
    }
    [HttpDelete("EliminarGestion")]
    public async Task<IActionResult> EliminarGestion(int id)
    {
      await _gestionRepository.EliminarGestion(id);
      return Ok();
    }
  }
}
