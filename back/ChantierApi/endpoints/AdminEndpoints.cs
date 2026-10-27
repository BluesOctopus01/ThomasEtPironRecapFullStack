using ChantierApi.models;
using Microsoft.AspNetCore.Identity;

namespace ChantierApi.Endpoints;

public static class AdminEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/admin/utilisateurs", async (
            CreateUserRequest request,
            UserManager<ApplicationUser> userManager) =>
        {
            // Seuls ces deux rôles peuvent être attribués
            if (request.Role is not ("Responsable" or "Ouvrier"))
                return Results.BadRequest("Rôle invalide.");

            // Créer l'utilisateur
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(
                user, request.Password
            );

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            // Attribuer son rôle
            var roleResult = await userManager.AddToRoleAsync(
                user, request.Role
            );

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return Results.Problem("Impossible d'attribuer le rôle.");
            }

            return Results.Ok(new
            {
                user.Id,
                user.Email,
                request.Role
            });
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Administrateur")
        );
    }
}

// Données attendues dans la requête HTTP
public record CreateUserRequest(
    string Email,
    string Password,
    string Role
);
