using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFirebase.Api.Dtos.Login;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Pagination;
using NetFirebase.Api.Services.Authentication;
using NetFirebase.Api.Vms;

namespace NetFirebase.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public UsuarioController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] UsuarioRegisterRequestDto request)
    {
        return await _authenticationService.RegisterAsync(request);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequestDto request)
    {
        return await _authenticationService.LoginAsync(request);
    }

    [AllowAnonymous]
    [HttpGet("paginationv1")]
    public async Task<ActionResult<PagedResults<Usuario>>> GetPaginationV1([FromQuery] PaginationParams request)
    {
        var resultados = await _authenticationService.GetPaginationVersion1(request);
        return Ok(resultados);
    }

    [AllowAnonymous]
    [HttpGet("paginationv2")]
    public async Task<ActionResult<PagedResults<UsuarioVm>>> GetPaginationV2([FromQuery] PaginationParams request)
    {
        var resultados = await _authenticationService.GetPaginationVersion2(request);
        return Ok(resultados);
    }
}
