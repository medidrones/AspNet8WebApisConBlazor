using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Permisos;

public class PermisoService : IPermisoService
{    
    private readonly DatabaseContext _context;

    public PermisoService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermisosAsync(string userId)
    {
        ICollection<Role>[] roles = await _context.Set<Usuario>()
            .Include(x => x.Roles!)
            .ThenInclude(x => x.Permisos)
            .Where( x => x.FirebaseId == userId)
            .Select(x => x.Roles!)
            .ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permisos!)
            .Select(x => x.Nombre)
            .ToHashSet();
    }
}