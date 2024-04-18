using Microsoft.AspNetCore.Mvc;
using Epistle.Services;

namespace Epistle.Controllers;

[Route("m")]
public class MissiveController(IDocumentService documents) : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("{id:regex(^[[1-9A-Za-z]]+$)}")]
    public async Task<IActionResult> Get(string id)
    {
        var uri = Request.ToInternalUri();
        var model = await documents.GetObjectAsync(uri);

        return Contextualize(model);
    }
}
