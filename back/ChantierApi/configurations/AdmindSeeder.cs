
using Microsoft.AspNetCore.Identity;
using ChantierApi.models;

namespace ChantierApi.Configuration;

public static class AdminSeeder
{
    public static async Task SeedAsync(WebApplication app)
    {
        // Uniquement pour notre environnement de développement
        if (!app.Environment.IsDevelopment())
            return;

        // Récupérer les identifiants dans les User Secrets
        var email = app.Configuration["BootstrapAdmin:Email"]
            ?? throw new InvalidOperationException("Email admin manquant.");

        var password = app.Configuration["BootstrapAdmin:Password"]
            ?? throw new InvalidOperationException("Mot de passe admin manquant.");

        // Récupérer le service qui gère les utilisateurs
        using var scope = app.Services.CreateScope();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        // Vérifier si notre administrateur existe déjà
        var admin = await userManager.FindByEmailAsync(email);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }

        // Attribuer le rôle Administrateur si nécessaire
        if (!await userManager.IsInRoleAsync(admin, "Administrateur"))
        {
            var result = await userManager.AddToRoleAsync(
                admin, "Administrateur"
            );

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }
    }
}
