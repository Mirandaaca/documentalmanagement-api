using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Persistence.Carrera;
using Project.Application.Interfaces.Persistence;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CarreraController : ControllerBase
  {
    private readonly ICarreraRepository _carreraRepository;
    public CarreraController(ICarreraRepository carreraRepository)
    {
      _carreraRepository = carreraRepository;
    }
    [HttpPost("GuardarCarrera")]
    public async Task<IActionResult> CrearCarrera([FromBody] CreateCarreraDTO carrera)
    {
      await _carreraRepository.GuardarCarrera(carrera);
      return Ok();
    }
    [HttpGet("ObtenerCarreras")]
    public async Task<IActionResult> ObtenerCarreras()
    {
      var carreras = await _carreraRepository.ObtenerCarreras();
      return Ok(carreras);
    }
    [HttpGet("ObtenerCarreraPorId")]
    public async Task<IActionResult> ObtenerCarreraPorId(int id)
    {
      var carrera = await _carreraRepository.ObtenerCarreraPorId(id);
      return Ok(carrera);
    }
    [HttpPut("ActualizarCarrera")]
    public async Task<IActionResult> ActualizarCarrera([FromBody] UpdateCarreraDTO carrera)
    {
      await _carreraRepository.ActualizarCarrera(carrera);
      return Ok();
    }
    [HttpDelete("EliminarCarrera")]
    public async Task<IActionResult> EliminarCarrera(int id)
    {
      await _carreraRepository.EliminarCarrera(id);
      return Ok();
    }
  }
}
