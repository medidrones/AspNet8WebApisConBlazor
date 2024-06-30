using blazor.Auth.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace blazor.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(
        HttpClient httpClient, 
        AuthenticationStateProvider authenticationStateProvider, 
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<string> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/Usuario/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            return null!;
        }

        var loginResult = await response.Content.ReadAsStringAsync();

        await _localStorage.SetItemAsStringAsync("authToken", loginResult);
        ((ApiAuthenticationStateProvider) _authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email!);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider) _authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string> Register(RegisterModel registerModel)
    {
        var registerAsJson = JsonSerializer.Serialize(registerModel);
        var response = await _httpClient.PostAsync("api/Usuario/register", new StringContent(registerAsJson, Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode) 
        {
            return null!;
        }

        var registerResult = await response.Content.ReadAsStringAsync();

        return registerResult;
    }
}
