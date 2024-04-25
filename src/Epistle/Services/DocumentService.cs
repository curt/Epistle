using Epistle.Repositories;

namespace Epistle.Services;

public class DocumentService(IDocumentRepository repository) : IDocumentService
{
    private readonly IDocumentRepository _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Object?> GetObjectAsync(Uri uri) =>
        await Task.Run(() => _repository.ObjectsQueryable().Where(x => x.Id == uri).FirstOrDefault());

    public async Task<Actor?> GetActorAsync(Uri uri) =>
        await Task.Run(() => _repository.ActorsQueryable().Where(x => x.Id == uri).FirstOrDefault());
}
