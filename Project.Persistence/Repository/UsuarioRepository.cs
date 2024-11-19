using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Application.DTOs.Infraestructure.Persistence.Usuario;
using Project.Application.Interfaces.Persistence;
using Project.Domain.Entities;
using Project.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
  public class UsuarioRepository : IUsuarioRepository
  {
    private readonly DocumentalManagement _context;
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private IConfiguration _configuration;
    public UsuarioRepository(DocumentalManagement context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
      _configuration = configuration;
      _userManager = userManager;
      _roleManager = roleManager;
      _context = context;
    }
    public async Task ActualizarUsuario(UpdateUsuarioDTO usuario)
    {
      Usuario objUsuario = await _userManager.FindByIdAsync(usuario.Id);
      if (objUsuario is null)
      {
        throw new ApplicationException("Usuario no encontrado");
      }
      objUsuario.Nombre = usuario.Nombre;
      objUsuario.Apellido = usuario.Apellido;
      objUsuario.Email = usuario.Email;
      objUsuario.PasswordHash = usuario.Password;
      objUsuario.FechaNacimiento = usuario.FechaNacimiento;
      await _userManager.RemoveFromRoleAsync(objUsuario, usuario.Role);
      await _userManager.AddToRoleAsync(objUsuario, usuario.Role);
      await _userManager.UpdateAsync(objUsuario);
    }

    public async Task EliminarUsuario(string id)
    {
      Usuario usuario = await _userManager.FindByIdAsync(id);
      if (usuario is null)
      {
        throw new ApplicationException("Usuario no encontrado");
      }
      await _userManager.DeleteAsync(usuario);
    }

    public async Task<ReadUsuarioRegistrationInformationDTO> GuardarUsuario(CreateUsuarioDTO usuario)
    {
      bool existeUsuario = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
      if (existeUsuario)
      {
        throw new ApplicationException("El usuario con ese email ya existe");
      }
      //var objUsuario = _mapper.Map<Usuario>(usuario);
      Usuario objUsuario = new Usuario
      {
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Email = usuario.Email,
        UserName = usuario.Email,
        FechaNacimiento = usuario.FechaNacimiento,
        EmailConfirmed = true,
        PhoneNumberConfirmed = true
      };
      var result = await _userManager.CreateAsync(objUsuario, usuario.Password);
      if (!result.Succeeded)
      {
        throw new ApplicationException("Error al crear el usuario");
      }
      await _userManager.AddToRoleAsync(objUsuario, usuario.Role);
      var usuarioEncontrado = await _userManager.FindByEmailAsync(usuario.Email);
      ReadUsuarioRegistrationInformationDTO usuarioDTO = new ReadUsuarioRegistrationInformationDTO
      {
        Id = usuarioEncontrado.Id,
        Username = usuarioEncontrado.UserName,
        Password = usuario.Password,
        Role = usuario.Role
      };
      return usuarioDTO;
    }

    public async Task<ReadUsuarioDTO> ObtenerUsuarioPorId(string id)
    {
      Usuario usuario = await _userManager.FindByIdAsync(id);
      var role = await _userManager.GetRolesAsync(usuario);
      ReadUsuarioDTO usuarioDTO = new ReadUsuarioDTO
      {
        Id = usuario.Id,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Email = usuario.Email,
        FechaDeNacimiento = usuario.FechaNacimiento,
        Password = usuario.PasswordHash,
        Role = role.FirstOrDefault()
      };
      return usuarioDTO;
    }

    public async Task<List<ReadUsuarioDTO>> ObtenerUsuarios()
    {

      List<Usuario> listaUsuarios = await _context.Usuarios.ToListAsync();
      List<ReadUsuarioDTO> listaUsuariosDTO = new List<ReadUsuarioDTO>();
      foreach (Usuario usuario in listaUsuarios)
      {
        var role = await _userManager.GetRolesAsync(usuario);
        ReadUsuarioDTO usuarioDTO = new ReadUsuarioDTO
        {
          Id = usuario.Id,
          Nombre = usuario.Nombre,
          Apellido = usuario.Apellido,
          Email = usuario.Email,
          FechaDeNacimiento = usuario.FechaNacimiento,
          Role = role.FirstOrDefault()
        };
        listaUsuariosDTO.Add(usuarioDTO);
      }
      return listaUsuariosDTO;
    }
  }
}
