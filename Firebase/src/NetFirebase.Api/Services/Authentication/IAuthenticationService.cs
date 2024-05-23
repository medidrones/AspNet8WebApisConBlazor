using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;

namespace NetFirebase.Api.Services.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(UsuarioRegisterRequestDto request);
    Task<string> LoginAsync(LoginRequestDto request);
}
