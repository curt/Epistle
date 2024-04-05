namespace Epistle.Services;

public interface IDocumentService
{
    public Task<ActivityPub.Object> GetObjectAsync(Uri uri);
    public Task InsertOneObjectAsync(ActivityPub.Object obj);
}