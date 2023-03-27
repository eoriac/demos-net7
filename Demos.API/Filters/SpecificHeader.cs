using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.API.Filters
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
