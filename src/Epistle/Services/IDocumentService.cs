using Epistle.ActivityPub;

using Object = Epistle.ActivityPub.Object;

namespace Epistle.Services;

public interface IDocumentService
{
    Task<Object?> GetObjectAsync(Uri uri);
    Task<Actor?> GetActorAsync(Uri uri);
}