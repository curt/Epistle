using MongoDB.Bson.Serialization.Conventions;
using Epistle.Models;
using Epistle.Services;

// Register MongoDB BSON conventions.
ConventionRegistry.Register(
    "EpistleBsonMappingConventions",
    new ConventionPack {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true)
    },
    t => t.FullName!.StartsWith("Epistle.ActivityPub.")
);

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DocumentDatabaseSettings>(
    builder.Configuration.GetSection("DocumentDatabase")
)
.AddScoped<IDocumentService, DocumentService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
