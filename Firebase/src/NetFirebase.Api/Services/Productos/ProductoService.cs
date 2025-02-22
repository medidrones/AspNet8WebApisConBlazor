using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Extensions;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Pagination;
using NetFirebase.Api.Vms;

namespace NetFirebase.Api.Services.Productos;

public class ProductoService : IProductoService
{
    private readonly DatabaseContext _context;
    private readonly IPagedList _paginacion;

    public ProductoService(DatabaseContext context, IPagedList paginacion)
    {
        _context = context;
        _paginacion = paginacion;
    }

    public async Task CreateProducto(Producto producto)
    {
        try
        {
            await _context.Database.ExecuteSqlAsync(@$"
                CALL sp_insert_producto({producto.Precio}, {producto.Nombre}, {producto.Descripcion});
            ");
        }
        catch (Exception ex) 
        {
            throw new Exception("Error en la insercion del producto", ex);
        }
              
    }

    public async Task DeleteProducto(int id)
    {
        try
        {
            await _context.Database.ExecuteSqlAsync($@"
                CALL sp_delete_producto({id})
            ");
        }
        catch (Exception ex)
        {
            throw new Exception("Error eliminado producto", ex);
        }
    }

    public async Task<IEnumerable<Producto>> GetAllProductos()
    {
        return await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM fx_query_producto_all()
        ").ToListAsync();
    }

    public async Task<PagedResults<ProductoVm>> GetPagination(PaginationParams request)
    {
        /* var query = _context.Database.SqlQuery<ProductoVm>(@$"
             SELECT * FROM ""Productos""

         ");*/

        IQueryable<Producto> query = _context.Productos;

        if(!string.IsNullOrEmpty(request.Search))
        {
            query = _context.Productos.Where(x => 
                x.Nombre!.Contains(request.Search!) || 
                x.Descripcion!.Contains(request.Search));
        }

        return await _paginacion.CreatePagedEntryAndGenericResults<Producto, ProductoVm>(
            query,
            request.PageNumber,
            request.PageSize,
            request.OrderBy!,
            request.OrderAsc
        );
    }

    public async Task<Producto> GetProductoById(int id)
    {
        var resultado = await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM fx_query_producto_by_id({id})
        ").ToListAsync();

        var producto = resultado.First();

        return producto;
    }

    public async Task<List<Producto>> GetProductoByNombre(string nombre)
    {
       return await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM fx_query_producto_by_nombre({nombre})
        ").ToListAsync();      
    }

    public Task<bool> SaveChanges()
    {
        throw new NotImplementedException();
    }

    public async Task UpdateProducto(Producto producto)
    {
        try
        {
            await _context.Database.ExecuteSqlAsync(@$"
                CALL sp_update_producto({producto.Id}, {producto.Precio}, {producto.Nombre}, {producto.Descripcion});");
        }
        catch (Exception ex)
        {
            throw new Exception("No se pudo actualizar el producto", ex);
        }
    }
}
