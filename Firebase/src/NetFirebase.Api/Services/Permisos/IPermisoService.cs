namespace NetFirebase.Api.Services.Permisos;

public interface IPermisoService
{
    Task<HashSet<string>> GetPermisosAsync(string userId);
}