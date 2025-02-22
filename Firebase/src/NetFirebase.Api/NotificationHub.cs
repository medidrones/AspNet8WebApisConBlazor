using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NetFirebase.Api;

[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
    public override Task OnConnectedAsync()
    {
        Clients.Client(Context.ConnectionId).RecibeNotification($"Gracias por tomar este repo {Context.User?.Identity?.Name}");

        return base.OnConnectedAsync();
    }
}
