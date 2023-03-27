namespace Demo.API.Services
{
    public interface INotificationService
    {
        void Send(string subject, string body);
    }
}