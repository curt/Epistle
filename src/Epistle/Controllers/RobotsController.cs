using Microsoft.AspNetCore.Mvc;
using Epistle.Services;
using Epistle.Domain.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace Epistle.Controllers;

[Route("/")]
public class RobotsController(IConfiguration config) : ControllerBase
{
    [Route("robots.txt")]
    public IActionResult RobotsTxt()
    {
        var response = new StringBuilder();
        var arachnophobic = config.GetValue(typeof(bool), "Arachnophobic");

        if (arachnophobic is not null && (bool)arachnophobic)
        {
            response.AppendLine("User-agent: *");
            response.AppendLine("Disallow: / ");
        }

        return Content(response.ToString());
    }
}
