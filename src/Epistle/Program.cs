using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Epistle;
using Epistle.Services;
using Epistle.Repositories;
using Epistle.ActivityPub;

// Register MongoDB BSON conventions.
ConventionRegistry.Register(
    "EpistleBsonMappingConventions",
    new ConventionPack {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true)
    },
    t => t.FullName!.StartsWith("Epistle.ActivityPub.")
);

BsonSerializer.RegisterSerializer(new EnumerableTripleBsonSerializer());

BsonClassMap.RegisterClassMap<Core>
(
    map =>
    {
        map.AutoMap();
        map.UnmapMember(m => m.JsonLdContext);
        map.MapExtraElementsMember(m => m.ExtraElements);
    }
);

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MvcRazorRuntimeCompilationOptions>
(
    options =>
    {
        var index = 0;
        var assemblies = builder.Configuration.GetValue<IEnumerable<string>>("Themes:Assemblies")!;
        foreach (var assembly in assemblies)
        {
            options.FileProviders.Insert(index++, new EmbeddedFileProvider(Assembly.Load(assembly)));
        }
    }
);

builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

// Add services to the container.
builder.Services.AddControllersWithViews
(
    options =>
    {
        // See https://stackoverflow.com/a/59813295
        var jsonInputFormatter = options.InputFormatters
            .OfType<SystemTextJsonInputFormatter>()
            .Single();

        jsonInputFormatter.SupportedMediaTypes.Add("application/ld+json");
        jsonInputFormatter.SupportedMediaTypes.Add("application/activity+json");
    }
)
.AddJsonOptions
(
    options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.TypeInfoResolver = new AlphabeticalOrderJsonTypeInfoResolver();
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
