using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Productos;

public interface IProductoService
{
    Task<IEnumerable<Producto>> GetAllProductos();
    Task<Producto> GetProductoById(int id);
    Task<List<Producto>> GetProductoByNombre(string nombre);
    Task CreateProducto(Producto producto);
    Task UpdateProducto(Producto producto);
    Task DeleteProducto(int id);
    Task<bool> SaveChanges();
}
