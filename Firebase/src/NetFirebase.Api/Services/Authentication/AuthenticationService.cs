using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Models;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly DatabaseContext _context;

    public AuthenticationService(HttpClient httpClient, DatabaseContext context)
    {
        _httpClient = httpClient;
        _context = context;
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
