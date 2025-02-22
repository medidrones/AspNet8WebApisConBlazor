namespace NetFirebase.Api;

public interface INotificationClient
{
    Task RecibeNotification(string message);
}
