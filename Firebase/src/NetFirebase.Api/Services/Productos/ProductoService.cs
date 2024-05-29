using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Productos;

public class ProductoService : IProductoService
{
    private readonly DatabaseContext _context;

    public ProductoService(DatabaseContext context)
    {
        _context = context;
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
