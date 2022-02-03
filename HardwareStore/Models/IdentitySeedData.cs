using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminRoleName = "admin";
        private const string adminName = "Admin";
        private const string adminPassword = "qwertY123#";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityResult result;

            IdentityRole role = await roleManager.FindByNameAsync(adminRoleName);
            if (role == null)
            {
                role = new(adminRoleName);
                result = await roleManager.CreateAsync(role);
            }

            IdentityUser user = await userManager.FindByNameAsync(adminName);
            if (user == null)
            {
                user = new(adminName);
                result = await userManager.CreateAsync(user, adminPassword);
                result = await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }
    }
}
