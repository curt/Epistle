namespace Epistle.Services;

public interface IDocumentService
{
    Task<Object?> GetObjectAsync(Uri uri);
    Task<Actor?> GetActorAsync(Uri uri);
}