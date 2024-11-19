using Microsoft.AspNetCore.Identity;
using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Identity.Seeds
{
  public static class DefaultRoles
  {
    public static async Task SeedAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
    {
      await roleManager.CreateAsync(new IdentityRole("Administrador"));
    }
  }
}
