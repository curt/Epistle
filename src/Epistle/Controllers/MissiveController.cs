using Microsoft.AspNetCore.Mvc;
using Epistle.Services;
using Epistle.Domain.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;

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
        var obj = await documents.GetObjectAsync(uri);

        if (obj is not null)
        {
            var attributedTo = obj.AttributedTo?.FirstOrDefault()?.Uri;

            if (attributedTo is not null)
            {
                var actor = await documents.GetActorAsync(attributedTo);

                if (actor is not null)
                {
                    obj = obj.Publicize(Request.ToEndpoint());
                    actor = actor.Publicize(Request.ToEndpoint());
                    var model = new ObjectViewModel(obj, actor);
                    return Contextualize(() => View(model), () => Json(Contextify(obj)));
                }
            }
        }

        return NotFound();
    }
}
