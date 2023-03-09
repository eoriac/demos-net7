namespace DemoSesion3.Services
{
    public class CloudNotificationService : INotificationService
    {
        private readonly string notificationService = string.Empty;

        public CloudNotificationService(IConfiguration configuration)
        {
            this.notificationService = configuration["Notification:ServiceEndpoint"];
        }

        public void Send(string subject, string body)
        {
            Console.WriteLine($"Send cloud notification from {nameof(CloudNotificationService)}");
            Console.WriteLine($"with {subject}: {body}");
            Console.WriteLine($"to {notificationService}");
        }
    }
}
