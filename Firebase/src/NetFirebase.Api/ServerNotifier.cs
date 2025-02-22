using Microsoft.AspNetCore.SignalR;
using NetFirebase.Api.Services.Authentication;
using NetFirebase.Api.Services.Productos;

namespace NetFirebase.Api;

public class ServerNotifier : BackgroundService
{
    private static readonly TimeSpan Periodo = TimeSpan.FromSeconds(5);

    private readonly IHubContext<NotificationHub, INotificationClient> _contextSR;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ServerNotifier> _logger;

    public ServerNotifier(
        IHubContext<NotificationHub, INotificationClient> contextSR,
        ILogger<ServerNotifier> logger,
        IServiceScopeFactory scopeFactory)
    {
        _contextSR = contextSR;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Periodo);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.Now;
            _logger.LogInformation($"Ejecutado {nameof(ServerNotifier)} {dateTime}");

            using var scope = _scopeFactory.CreateScope();
            var authenticationService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
            var productoService = scope.ServiceProvider.GetRequiredService<IProductoService>();
            var usuario = await authenticationService.GetUserByEmail("medicodetest@gmail.com");

            if (usuario is  not null)
            {
                var productos = await productoService.GetProductoByNombre("A");
                var random = new Random();
                int indiceRandom = random.Next(productos.Count);
                var producto = productos[indiceRandom];

                await _contextSR.Clients.User(usuario.FirebaseId!)
                    .RecibeNotification($"Producto del dia para comprar: {producto.Nombre} - Paga solo {producto.Precio}");
            }
        }
    }
}
