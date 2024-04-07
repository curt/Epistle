using Microsoft.AspNetCore.Mvc;
using Epistle.Services;
using Microsoft.AspNetCore.Http.Extensions;

namespace Epistle.Controllers;

[Route("m")]
public class MissiveController(IDocumentService documents) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("{id:regex(^[[1-9A-Za-z]]+$)}")]
    public async Task<IActionResult> Get(string id)
    {
        var uri = Request.ToInternalUri();
        var obj = await documents.GetObjectAsync(uri);

        if (obj is null)
            return NotFound();

        obj = obj.Publicize(Request.ToEndpoint());

        return View(obj);
    }
}
