using Bogus;
using NetFirebase.Api.Data;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Extensions;

public static class DataPruebaExtensions
{
    public static async void AddDataPrueba(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var dbContext = service.GetRequiredService<DatabaseContext>();

        if (!dbContext.Productos.Any())
        {
            var productoCollection = new List<Producto>();
            var faker = new Faker();

            for (var i = 1; i <= 1000; i++)
            {
                productoCollection.Add(new Producto
                {
                    Nombre = faker.Commerce.ProductName(),
                    Descripcion = faker.Commerce.ProductDescription(),
                    Precio = faker.Random.Decimal(100, 50000),
                });
            }

            await dbContext.Productos.AddRangeAsync(productoCollection);
            await dbContext.SaveChangesAsync();
        }
    }
}
