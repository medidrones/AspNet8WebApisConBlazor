using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Pagination;
using NetFirebase.Api.Vms;

namespace NetFirebase.Api.Services.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(UsuarioRegisterRequestDto request);
    Task<string> LoginAsync(LoginRequestDto request);
    Task<Usuario> GetUserByEmail(string email);
    Task<PagedResults<Usuario>> GetPaginationVersion1(PaginationParams request);
    Task<PagedResults<UsuarioVm>> GetPaginationVersion2(PaginationParams request);
}
