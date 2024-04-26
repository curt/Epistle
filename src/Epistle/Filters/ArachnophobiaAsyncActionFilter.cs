using Microsoft.AspNetCore.Mvc.Filters;

namespace Epistle.Filters;

public class ArachnophobiaAsyncActionFilter(IConfiguration config) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();

        var arachnophobic = config.GetValue<bool?>("Arachnophobic") ?? false;
        if (arachnophobic)
        {
            var response = context.HttpContext.Response;
            response.Headers.Append("X-Robots-Tag", "noindex, nofollow, noarchive");
        }
    }
}
