using Microsoft.AspNetCore.Mvc.Filters;

namespace Epistle.Filters;

public class ArachnophobiaAsyncActionFilter(IConfiguration config) : IAsyncActionFilter
{
    private bool Arachnophobic => config.GetValue<bool?>("Arachnophobic") ?? false;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();

        if (Arachnophobic)
        {
            var response = context.HttpContext.Response;
            response.Headers.Append("X-Robots-Tag", "noindex, nofollow, noarchive");
        }
    }
}
