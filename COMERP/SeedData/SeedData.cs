using COMERP.Entities;
using Microsoft.AspNetCore.Identity;

namespace COMERP.SeedData
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider sp)
        {
            //Initialize Role and User manager 
            var roleManager = sp.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed roles
            string[] roleNames = { "Admin", "User", "Developer" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles if they do not exist
                    var roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        // Handle role creation failure
                        throw new Exception($"Failed to create role {roleName}");
                    }
                }
            }
            // Seed a default admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@123",
                Email = "admin@gmail.com",
                FirstName = "System",
                LastName = "Admin",
                PhoneNumber = "01767988385",
            };
            // Seed a default admin Password
            string adminPassword = "admin@123";
            var user = await userManager.FindByEmailAsync(adminUser.Email);
            if (user == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdminUser.Succeeded)
                {
                    // Assign the "Admin" role to the admin user
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

    }
}
