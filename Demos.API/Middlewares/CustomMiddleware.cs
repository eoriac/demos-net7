namespace Demo.API.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate next;

        public CustomMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var headers = context.Response.Headers;

            headers.Add("X-Frame-Options", "DENY");
            headers.Add("X-XSS-Protection", "1; mode=block");
            headers.Add("X-Content-Type-Options", "nosniff");
            headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");


            await this.next(context);
        }
    }
}
