using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Application.DTOs.Infraestructure.Identity.Authentication;
using Project.Application.DTOs.Infraestructure.Persistence.Usuario;
using Project.Application.Interfaces.Identity;
using Project.Application.Wrappers;
using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Identity.Services
{
  public class UsuarioService : IUsuarioService
  {
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<Usuario> _signInManager;
    IConfiguration _configuration;
    public UsuarioService(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Usuario> signInManager, IConfiguration configuration)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _signInManager = signInManager;
      _configuration = configuration;
    }
    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
    {
      Usuario usuario = await _userManager.FindByEmailAsync(request.Email);

      if (usuario == null)
        throw new ApplicationException("Usuario con ese email no existe");


      var result = await _signInManager.PasswordSignInAsync(usuario, request.Password, false, false);

      if (!result.Succeeded)
      {
        throw new ApplicationException($"Las credenciales del usuario no son validas ${request.Email}.");
      }

      JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(usuario);

      AuthenticationResponse response = new AuthenticationResponse();
      response.IdUsuario = usuario.Id;
      response.JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
      response.Email = usuario.Email;
      response.UserName = usuario.UserName;

      var roles = await _userManager.GetRolesAsync(usuario);

      response.Rol = roles.FirstOrDefault();

      response.DatosUsuario = await ObtenerInformacionUsuario(response.Rol, usuario.Email);

      return new Response<AuthenticationResponse>(response, $"Usuario Autenticado {usuario.UserName}");
    }
    private async Task<ReadUserInformationDTO> ObtenerInformacionUsuario(string rol, string email)
    {
      ReadUserInformationDTO userInformation = new ReadUserInformationDTO();

      switch (rol)
      {
        case ("Administrador"):
          var usuarioAdmin = await _userManager.FindByEmailAsync(email);
          userInformation.Nombre = usuarioAdmin.Nombre;
          userInformation.FechaNacimiento = usuarioAdmin.FechaNacimiento;
          userInformation.Apellido = usuarioAdmin.Apellido;
          break;
      }
      return userInformation;
    }
    private async Task<JwtSecurityToken> GenerateJWTToken(Usuario usuario)
    {
      var userClaims = await _userManager.GetClaimsAsync(usuario);
      var roles = await _userManager.GetRolesAsync(usuario);

      var roleClaims = new List<Claim>();

      for (int i = 0; i < roles.Count; i++)
      {
        roleClaims.Add(new Claim("roles", roles[i]));
      }

      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            }
      .Union(userClaims)
      .Union(roleClaims);

      var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecretKey"]));
      var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

      var jwtSecurityToken = new JwtSecurityToken(
          issuer: _configuration["JWTSettings:Issuer"],
          audience: _configuration["JWTSettings:Audience"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWTSettings:AccessExpiration"])),
          signingCredentials: signingCredentials
      );

      return jwtSecurityToken;
    }
  }
}
