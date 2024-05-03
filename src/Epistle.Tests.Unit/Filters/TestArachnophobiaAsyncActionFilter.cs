using Epistle.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Epistle.Tests.Unit.Filters;

[TestClass]
public class TestArachnophobiaAsyncActionFilter
{
    [TestMethod]
    public async Task TestArachnophobic()
    {
        await TestArachnophobicByConfig("true", Assert.IsTrue);
    }

    [TestMethod]
    public async Task TestNotArachnophobic()
    {
        await TestArachnophobicByConfig("false", Assert.IsFalse);
    }

    private async Task TestArachnophobicByConfig(string configSetting, Action<bool> assertion)
    {
        var mockConfigService = new Mock<IConfiguration>();
        var mockConfigSection = new Mock<IConfigurationSection>();
        mockConfigSection.Setup(cs => cs.Value).Returns(configSetting);
        mockConfigService.Setup(cs => cs.GetSection("Arachnophobic")).Returns(mockConfigSection.Object);
        var actionFilter = new ArachnophobiaAsyncActionFilter(mockConfigService.Object);

        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext
        (
            httpContext,
            new RouteData(),
            new ActionDescriptor(),
            new ModelStateDictionary()
        );
        var actionExecutingContext = new ActionExecutingContext
        (
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            null!
        );
        var actionExecutedContext = new ActionExecutedContext
        (
            actionContext,
            new List<IFilterMetadata>(),
            null!
        );

        await actionFilter.OnActionExecutionAsync(actionExecutingContext, () => Task.Run(() => actionExecutedContext));

        var headers = httpContext?.Response?.Headers;
        Assert.IsNotNull(headers);

        var robots = headers.Where(x => x.Key.Equals("X-Robots-Tag", StringComparison.OrdinalIgnoreCase));
        foreach (var robot in robots)
        {
            string val = robot.Value!;
            assertion(val.Contains("noindex"));
            assertion(val.Contains("nofollow"));
            assertion(val.Contains("noarchive"));
        }
    }
}