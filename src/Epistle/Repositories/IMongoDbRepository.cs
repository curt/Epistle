using Epistle.ActivityPub;

namespace Epistle.Repositories;

public interface IMongoDbRepository
{
    IQueryable<ActivityPub.Object> FindAllObjects();
    IQueryable<Activity> FindAllActivities();
    IQueryable<Actor> FindAllActors();
}