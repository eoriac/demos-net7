using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoSesion3.Filters
{
    public class SpecificHeader : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var headers = context.HttpContext.Response.Headers;

            headers.Add("x-game-header", "somespecificvalue");

            base.OnResultExecuting(context);
        }
    }
}
