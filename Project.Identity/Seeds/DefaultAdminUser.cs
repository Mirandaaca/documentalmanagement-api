using Microsoft.AspNetCore.Identity;
using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Identity.Seeds
{
  public static class DefaultAdminUser
  {
    public static async Task SeedAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
    {
      //Seed Default Admin User
      var defaultUser = new Usuario
      {
        UserName = "cristopher",
        Email = "cristopher780c@gmail.com",
        Nombre = "Cristopher Adrian",
        Apellido = "Miranda Aramayo",
        PhoneNumber = "76081638",
        FechaNacimiento = new DateTime(2002, 12, 10),
        EmailConfirmed = true,
        PhoneNumberConfirmed = true
      };

      if (userManager.Users.All(u => u.Id != defaultUser.Id))
      {
        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
          await userManager.CreateAsync(defaultUser, "Cristopher123*");
          await userManager.AddToRoleAsync(defaultUser, "Administrador");
        }
      }
    }
  }
}
