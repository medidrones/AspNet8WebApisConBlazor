using Microsoft.AspNetCore.SignalR;

namespace NetFirebase.Api;

public class ServerNotifier : BackgroundService
{
    private static readonly TimeSpan Periodo = TimeSpan.FromSeconds(5);
    private readonly IHubContext<NotificationHub, INotificationClient> _contextSR;    
    private readonly ILogger<ServerNotifier> _logger;

    public ServerNotifier(
        IHubContext<NotificationHub, INotificationClient> contextSR, 
        ILogger<ServerNotifier> logger)
    {
        _contextSR = contextSR;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Periodo);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.Now;
            _logger.LogInformation($"Ejecutado {nameof(ServerNotifier)} {dateTime}");

            await _contextSR.Clients.All.RecibeNotification($"Servidor time = {dateTime}");
        }
    }
}
