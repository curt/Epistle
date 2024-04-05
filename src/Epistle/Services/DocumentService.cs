using Epistle.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Epistle.Services;

public class DocumentService : IDocumentService
{
    private IMongoCollection<ActivityPub.Object> ObjectsCollection { get; }

    public DocumentService(IOptions<DocumentDatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        ObjectsCollection = mongoDatabase.GetCollection<ActivityPub.Object>(databaseSettings.Value.ObjectsCollectionName);
    }

    public async Task<ActivityPub.Object> GetObjectAsync(Uri uri) =>
        await ObjectsCollection.Find(x => x.Id == uri).FirstOrDefaultAsync();

    public async Task InsertOneObjectAsync(ActivityPub.Object obj) =>
        await ObjectsCollection.InsertOneAsync(obj);
}
