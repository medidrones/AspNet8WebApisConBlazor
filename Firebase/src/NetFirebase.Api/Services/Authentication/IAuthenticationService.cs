using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(UsuarioRegisterRequestDto request);
    Task<string> LoginAsync(LoginRequestDto request);
    Task<Usuario> GetUserByEmail(string email);
}
