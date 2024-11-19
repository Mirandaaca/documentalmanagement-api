using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
using Project.Application.Interfaces.Persistence;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EstudianteController : ControllerBase
  {
    private readonly IEstudianteRepository _estudianteRepository;
    public EstudianteController(IEstudianteRepository estudianteRepository)
    {
      _estudianteRepository = estudianteRepository;
    }
    [HttpPost("GuardarEstudiante")]
    public async Task<IActionResult> CrearEstudiante([FromBody] CreateEstudianteDTO estudiante)
    {
      await _estudianteRepository.GuardarEstudiante(estudiante);
      return Ok();
    }
    [HttpGet("ObtenerEstudiantes")]
    public async Task<IActionResult> ObtenerEstudiantes()
    {
      var estudiantes = await _estudianteRepository.ObtenerEstudiantes();
      return Ok(estudiantes);
    }
    [HttpGet("ObtenerEstudiantePorId")]
    public async Task<IActionResult> ObtenerEstudiantePorId(int id)
    {
      var estudiante = await _estudianteRepository.ObtenerEstudiantePorId(id);
      return Ok(estudiante);
    }
    [HttpPut("ActualizarEstudiante")]
    public async Task<IActionResult> ActualizarEstudiante([FromBody] UpdateEstudianteDTO estudiante)
    {
      await _estudianteRepository.ActualizarEstudiante(estudiante);
      return Ok();
    }
    [HttpDelete("EliminarEstudiante")]
    public async Task<IActionResult> EliminarEstudiante(int id)
    {
      await _estudianteRepository.EliminarEstudiante(id);
      return Ok();
    }
    [HttpGet("ObtenerDocumentosFaltantesYPresentadosPorCategoriaDeEstudiantes")]
    public async Task<IActionResult> ObtenerDocumentosFaltantesYPresentadosPorCategoriaDeEstudiantes()
    {
      var estudiantes = await _estudianteRepository.ObtenerEstudiantesConDocumentosFaltantesYPresentados();
      return Ok(estudiantes);
    }
    [HttpGet("ObtenerDocumentosFaltantesPorCategoriaDeEstudiantes")]
    public async Task<IActionResult> ObtenerDocumentosFaltantesPorCategoriaDeEstudiantes()
    {
      var estudiantes = await _estudianteRepository.ObtenerEstudiantesConDocumentosFaltantesPorCategoria();
      return Ok(estudiantes);
    }
    [HttpGet("ObtenerDocumentosPresentadosPorCategoriaDeEstudiantes")]
    public async Task<IActionResult> ObtenerDocumentosPresentadosPorCategoriaDeEstudiantes()
    {
      var estudiantes = await _estudianteRepository.ObtenerEstudiantesConDocumentosPresentadosPorCategoria();
      return Ok(estudiantes);
    }
  }
}
