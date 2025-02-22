using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Models;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Pagination;
using NetFirebase.Api.Vms;

namespace NetFirebase.Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly DatabaseContext _context;
    private readonly IPagedList _paginacion;

    public AuthenticationService(HttpClient httpClient, DatabaseContext context, IPagedList paginacion)
    {
        _httpClient = httpClient;
        _context = context;
        _paginacion = paginacion;
    }

    public async Task<PagedResults<Usuario>> GetPaginationVersion1(PaginationParams request)
    {
        var query = _context.Usuarios
            .Include(x => x.Roles)!
            .ThenInclude(x => x.Permisos);

        return await _paginacion.CreatePagedGenericResults<Usuario>(
            query, 
            request.PageNumber, 
            request.PageSize, 
            request.OrderBy!, 
            request.OrderAsc);
    }

    public async Task<PagedResults<UsuarioVm>> GetPaginationVersion2(PaginationParams request)
    {
        var query = _context.Database.SqlQuery<UsuarioVm>(@$"
            select 
                usr.""Id"",
                usr.""Email"",
                usr.""FullName"",
                string_agg(rol.""Name"", ',') as ""Role"",
                string_agg(perm.""Nombre"", ',') as ""Permiso""
                From ""Usuarios"" as usr
                left join ""UsuarioRole"" as usrol ON usr.""Id""=usrol.""UsuarioId""
                left join ""Roles"" as rol ON rol.""Id""=usrol.""RoleId""
                left join ""RolePermiso"" as rolePermiso ON rolePermiso.""RoleId""=rol.""Id""
                left join ""Permisos"" as perm ON perm.""Id""=rolePermiso.""PermisoId""
                group by usr.""Id""
        ");

        return await _paginacion.CreatePagedGenericResults(
            query,
            request.PageNumber,
            request.PageSize,
            request.OrderBy!,
            request.OrderAsc);
    }

    public async Task<Usuario> GetUserByEmail(string email)
    {
        return await _context.Usuarios.Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<string> LoginAsync(LoginRequestDto request)
    {
        var credentials = new { request.Email, request.Password, returnSecureToken = true };
        var response = await _httpClient.PostAsJsonAsync("", credentials);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Credenciales erroneas");
        }

        var authFirebaseObject = await response.Content.ReadFromJsonAsync<AuthFirebase>();

        return authFirebaseObject!.IdToken!;
    }

    public async Task<string> RegisterAsync(UsuarioRegisterRequestDto request)
    {
        var userArgs = new UserRecordArgs { 
            DisplayName = request.FullNombre, 
            Email = request.Email, 
            Password = request.Password };

        var usuario = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

        _context.Usuarios.Add(new Usuario
        {            
            Email = request.Email,
            FullName = request.FullNombre,
            FirebaseId = usuario.Uid
        });

        await _context.SaveChangesAsync();

        return usuario.Uid;
    }
}
