using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Infraestructure.Identity.Authentication;
using Project.Application.DTOs.Infraestructure.Persistence.Usuario;
using Project.Application.Interfaces.Identity;
using Project.Application.Interfaces.Persistence;
using Project.Application.Wrappers;

namespace Project.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsuariosController : ControllerBase
  {
    private IUsuarioService _usuarioService;
    private IUsuarioRepository _usuarioRepository;
    public UsuariosController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository)
    {
      _usuarioService = usuarioService;
      _usuarioRepository = usuarioRepository;
    }
    [HttpPost("LogIn")]
    public async Task<IActionResult> LogIn(AuthenticationRequest request)
    {
      var response = await _usuarioService.AuthenticateAsync(request);
      return Ok(response);
    }
    [HttpPost("CrearUsuario")]
    public async Task<IActionResult> CrearUsuario(CreateUsuarioDTO usuario)
    {
      try
      {
        var response = await _usuarioRepository.GuardarUsuario(usuario);
        return Ok(new Response<ReadUsuarioRegistrationInformationDTO>(data: response, message: "Usuario guardado exitosamente"));
      }
      catch (Exception)
      {

        return StatusCode(500, new Response<bool>(message: "Ocurrió un error al guardar al usuario", succeded: false));
      }


    }
    [HttpGet("ObtenerUsuarios")]
    public async Task<IActionResult> ObtenerUsuarios()
    {
      try
      {
        var response = await _usuarioRepository.ObtenerUsuarios();
        return Ok(new Response<List<ReadUsuarioDTO>>(data: response));
      }
      catch (Exception)
      {
        return StatusCode(500, new Response<bool>(message: "Ocurrió un error al obtener los usuarios", succeded: false));
      }
    }
    [HttpGet("ObtenerUsuarioPorId")]
    public async Task<IActionResult> ObtenerUsuarioPorId(string id)
    {
      try
      {
        var response = await _usuarioRepository.ObtenerUsuarioPorId(id);
        return Ok(new Response<ReadUsuarioDTO>(data: response));
      }
      catch (Exception)
      {
        return StatusCode(500, new Response<bool>(message: "Ocurrió un error al obtener el usuario", succeded: false));
      }
    }
    [HttpPatch("ActualizarUsuario")]
    public async Task<IActionResult> ActualizarUsuario(UpdateUsuarioDTO usuario)
    {
      try
      {
        await _usuarioRepository.ActualizarUsuario(usuario);
        return Ok(new Response<bool>(message: "Usuario actualizado exitosamente", succeded: true));
      }
      catch (Exception)
      {
        return StatusCode(500, new Response<bool>(message: "Ocurrió un error al actualizar el usuario", succeded: false));
      }
    }
    [HttpDelete("EliminarUsuario")]
    public async Task<IActionResult> EliminarUsuario(string id)
    {
      try
      {
        await _usuarioRepository.EliminarUsuario(id);
        return Ok(new Response<bool>(message: "Usuario eliminado exitosamente", succeded: true));
      }
      catch (Exception)
      {
        return StatusCode(500, new Response<bool>(message: "Ocurrió un error al eliminar el usuario", succeded: false));
      }
    }

  }
}
