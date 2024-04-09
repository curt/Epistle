using Microsoft.AspNetCore.Mvc;
using Epistle.ActivityPub;

namespace Epistle.Controllers;

public class BaseController : Controller
{
    public static readonly string[] ActivityMediaTypes = [
        "application/ld+json",
        "application/activity+json",
        "application/json"
    ];

    protected IActionResult Contextualize(Func<IActionResult> html, Func<IActionResult> json)
    {
        var acceptHeaders = Request.GetTypedHeaders().Accept;
        var jsonAcceptHeader = acceptHeaders.FirstOrDefault(x => ActivityMediaTypes.Contains(x.MediaType.Value));

        if (jsonAcceptHeader != null)
        {
            Response.ContentType = jsonAcceptHeader.MediaType.Buffer;
            return json();
        }

        return html();
    }

    protected Core Contextify(Core core)
    {
        core.JsonLdContext = new EnumerableTriple() { new Triple(new Uri("https://www.w3.org/ns/activitystreams")) };
        return core;
    }
}
