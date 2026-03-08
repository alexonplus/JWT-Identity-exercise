using Microsoft.AspNetCore.Identity;
using Security.Models;

public static class DbSeeder
{
    public static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(Roles.Admin))
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

        if (!await roleManager.RoleExistsAsync(Roles.Teacher))
            await roleManager.CreateAsync(new IdentityRole(Roles.Teacher));

        if (!await roleManager.RoleExistsAsync(Roles.Student))
            await roleManager.CreateAsync(new IdentityRole(Roles.Student));

        var adminEmail = config["AdminSettings:Email"]!;
        var adminPassword = config["AdminSettings:Password"]!;

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }

            else
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }


        }
    }
}