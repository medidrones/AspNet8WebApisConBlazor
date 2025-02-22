using Microsoft.AspNetCore.Authorization;

namespace NetFirebase.Api.Authentication;

public class PermisoRequirement : IAuthorizationRequirement
{
    public PermisoRequirement(string permiso)
    {
        Permiso = permiso;
    }

    public string Permiso { get; }
}