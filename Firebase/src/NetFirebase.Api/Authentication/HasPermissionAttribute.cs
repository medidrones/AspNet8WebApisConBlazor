using Microsoft.AspNetCore.Authorization;
using NetFirebase.Api.Models.Enums;

namespace NetFirebase.Api.Authentication;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermisoEnum permiso) : base(policy: permiso.ToString())
    {        
    }   
}
