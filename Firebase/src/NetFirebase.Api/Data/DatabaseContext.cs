using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}
