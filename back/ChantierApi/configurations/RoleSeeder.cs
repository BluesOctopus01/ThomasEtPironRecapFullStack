
using Microsoft.AspNetCore.Identity;

namespace ChantierApi.Configuration;

public static class RoleSeeder
{
    public static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Responsable", "Ouvrier" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role)
                );

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Impossible de créer le rôle {role}"
                    );
                }
            }
        }
    }
}
