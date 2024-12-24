using System;
using Microsoft.AspNetCore.Identity;

namespace Repository.Extensions;

public static class RoleManagerExtensions
{
    public static async Task SeedRolesAsync(this RoleManager<IdentityRole<Guid>> roleManager)
    {
        string roleName = "Administrator";

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
    }
}
