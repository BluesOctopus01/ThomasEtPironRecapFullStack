using Microsoft.AspNetCore.Identity;
using ChantierApi.data;
using Microsoft.EntityFrameworkCore;
using ChantierApi.models;
using ChantierApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Récupérer notre chaîne de connexion
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connexion PostgreSQL introuvable.");

// Configurer Entity Framework Core avec PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization();

var app = builder.Build();

AuthEndpoints.Map(app);
DbCheckEndpoints.Map(app);

app.Run();
