using Epistle.ActivityPub;

namespace Epistle.Repositories;

public interface IDocumentRepository
{
    IQueryable<ActivityPub.Object> ObjectsQueryable();
    IQueryable<Activity> ActivitiesQueryable();
    IQueryable<Actor> ActorsQueryable();
}