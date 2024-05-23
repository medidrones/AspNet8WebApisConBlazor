using FirebaseAdmin.Auth;
using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Models;

namespace NetFirebase.Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> LoginAsync(LoginRequestDto request)
    {
        var credentials = new { request.Email, request.Password, returnSecureToken = true };
        var response = await _httpClient.PostAsJsonAsync("", credentials);
        var authFirebaseObject = await response.Content.ReadFromJsonAsync<AuthFirebase>();

        return authFirebaseObject!.IdToken!;
    }

    public async Task<string> RegisterAsync(UsuarioRegisterRequestDto request)
    {
        var userArgs = new UserRecordArgs { Email = request.Email, Password = request.Password };
        var usuario = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);
        
        return usuario.Uid;
    }
}
