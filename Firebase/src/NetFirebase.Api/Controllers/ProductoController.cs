using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFirebase.Api.Authentication;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Models.Enums;
using NetFirebase.Api.Pagination;
using NetFirebase.Api.Services.Productos;
using NetFirebase.Api.Vms;

namespace NetFirebase.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HasPermission(PermisoEnum.WriteUsuario)]
    [HttpPost]
    public async Task<ActionResult> CreateProducto([FromBody] Producto request)
    {
        await _productoService.CreateProducto(request);

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetAllProductos()
    {
        var resultados = await _productoService.GetAllProductos();

        return Ok(resultados);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductoById(int id)
    {
        var resultado = await _productoService.GetProductoById(id);

        return Ok(resultado);
    }

    [Authorize]
    [HttpGet("nombre/{nombre}")]
    public async Task<ActionResult> GetProductoByNombre(string nombre)
    {
        var resultado = await _productoService.GetProductoByNombre(nombre);

        return Ok(resultado);
    }

    [HasPermission(PermisoEnum.WriteUsuario)]
    [HttpPut]
    public async Task<ActionResult> UpdateProducto([FromBody] Producto request)
    {
        await _productoService.UpdateProducto(request);

        return Ok();
    }

    [HasPermission(PermisoEnum.WriteUsuario)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductoById(int id)
    {
        await _productoService.DeleteProducto(id);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("pagination")]
    public async Task<ActionResult<PagedResults<ProductoVm>>> GetPagination([FromQuery] PaginationParams request)
    {
        var resultados = await _productoService.GetPagination(request);
        return Ok(resultados);
    }
}
