using Microsoft.AspNetCore.Authorization;
using NetFirebase.Api.Services.Permisos;
using System.Security.Claims;

namespace NetFirebase.Api.Authentication;

public class PermisoAuthorizationHandler : AuthorizationHandler<PermisoRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermisoAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisoRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IPermisoService permisoService = scope.ServiceProvider.GetRequiredService<IPermisoService>();

        var permisos = await permisoService.GetPermisosAsync(userId!);

        if (permisos.Contains(requirement.Permiso))
        {
            context.Succeed(requirement);
        }
    }
}
