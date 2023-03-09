namespace DemoSesion3.Services
{
    public class LocalNotificationService : INotificationService
    {
        private readonly string notificationService = string.Empty;

        public LocalNotificationService(IConfiguration configuration)
        {
            this.notificationService = configuration["Notification:ServiceEndpoint"];
        }

        public void Send(string subject, string body)
        {
            Console.WriteLine($"Send local notification from {nameof(LocalNotificationService)}");
            Console.WriteLine($"with {subject}: {body}");
            Console.WriteLine($"to {notificationService}");
        }
    }
}
