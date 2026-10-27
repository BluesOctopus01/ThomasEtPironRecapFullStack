using ChantierApi.data;
using ChantierApi.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChantierApi.Configuration;

public static class ServiceConfiguration
{
    public static void Configure(WebApplicationBuilder builder)
    {
        // Configuration PostgreSQL
        var connectionString = builder.Configuration
            .GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connexion PostgreSQL introuvable."
            );

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        // Configuration Identity
        builder.Services
            .AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddAuthorization();
    }
}
