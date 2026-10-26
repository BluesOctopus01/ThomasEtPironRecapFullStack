
using Microsoft.EntityFrameworkCore;

namespace ChantierApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
}
