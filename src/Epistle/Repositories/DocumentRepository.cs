using Epistle.Repositories;
using MongoDB.Driver;

namespace Epistle;

public class DocumentRepository : IDocumentRepository
{
    private readonly IMongoDatabase _db;

    public DocumentRepository(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = configuration["MongoDB:ConnectionString"]
            ?? throw new InvalidOperationException("missing connection string");

        var client = new MongoClient(connectionString)
            ?? throw new InvalidOperationException("unable to create connection");

        var dbName = configuration["MongoDB:Database"]
            ?? throw new InvalidOperationException("missing database name");

        _db = client.GetDatabase(dbName)
            ?? throw new InvalidOperationException("unable to connect to database");
    }

    public IQueryable<Object> ObjectsQueryable()
    {
        return Objects.AsQueryable();
    }

    public IQueryable<Activity> ActivitiesQueryable()
    {
        return Activities.AsQueryable();
    }

    public IQueryable<Actor> ActorsQueryable()
    {
        return Actors.AsQueryable();
    }

    private IMongoCollection<Object> Objects => GetCollection<Object>("objects");

    private IMongoCollection<Activity> Activities => GetCollection<Activity>("activities");

    private IMongoCollection<Actor> Actors => GetCollection<Actor>("actors");

    private IMongoCollection<T> GetCollection<T>(string name) =>
        _db.GetCollection<T>(name)
            ?? throw new InvalidOperationException($"unable to find {name} collection");
}
