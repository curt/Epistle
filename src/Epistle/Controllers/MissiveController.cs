using Microsoft.AspNetCore.Mvc;
using Epistle.Services;

namespace Epistle.Controllers;

[Route("m")]
public class MissiveController : Controller
{
    private readonly ILogger<MissiveController> _logger;
    private readonly DocumentService _documents;

    public MissiveController(ILogger<MissiveController> logger, DocumentService documents)
    {
        _logger = logger;
        _documents = documents;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("{id:regex(^[[1-9A-Za-z]]+$)}")]
    public async Task<IActionResult> Get(string id)
    {
        var uri = new UriBuilder("http", "internal")
        {
            Path = Request.Path
        };

        var obj = await _documents.GetObjectAsync(uri.Uri);

        if (obj is null)
            return NotFound();

        return View(obj);
    }
}
