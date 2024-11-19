using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Persistence.Facultad;
using Project.Application.Interfaces.Persistence;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FacultadController : ControllerBase
  {
    private readonly IFacultadRepository _facultadRepository;
    public FacultadController(IFacultadRepository facultadRepository)
    {
      _facultadRepository = facultadRepository;
    }
    [HttpPost("GuardarFacultad")]
    public async Task<IActionResult> CrearFacultad([FromBody] CreateFacultadDTO facultad)
    {
      await _facultadRepository.GuardarFacultad(facultad);
      return Ok();
    }
    [HttpGet("ObtenerFacultades")]
    public async Task<IActionResult> ObtenerFacultades()
    {
      var facultades = await _facultadRepository.ObtenerFacultades();
      return Ok(facultades);
    }
    [HttpGet("ObtenerFacultadPorId")]
    public async Task<IActionResult> ObtenerFacultadPorId(int id)
    {
      var facultad = await _facultadRepository.ObtenerFacultadPorId(id);
      return Ok(facultad);
    }
    [HttpPut("ActualizarFacultad")]
    public async Task<IActionResult> ActualizarFacultad([FromBody] UpdateFacultadDTO facultad)
    {
      await _facultadRepository.ActualizarFacultad(facultad);
      return Ok();
    }
    [HttpDelete("EliminarFacultad")]
    public async Task<IActionResult> EliminarFacultad(int id)
    {
      await _facultadRepository.EliminarFacultad(id);
      return Ok();
    }
  }
}
