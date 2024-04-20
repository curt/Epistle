using Epistle.ActivityPub;
using Epistle.Repositories;
using MongoDB.Driver;

using Object = Epistle.ActivityPub.Object;

namespace Epistle.Services;

public class DocumentService(IMongoDbRepository repository) : IDocumentService
{
    private readonly IMongoDbRepository _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Object?> GetObjectAsync(Uri uri) =>
        await Task.Run(() => _repository.FindAllObjects().Where(x => x.Id == uri).FirstOrDefault());

    public async Task<Actor?> GetActorAsync(Uri uri) =>
        await Task.Run(() => _repository.FindAllActors().Where(x => x.Id == uri).FirstOrDefault());
}
