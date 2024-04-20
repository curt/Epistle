﻿using Epistle.ActivityPub;
using Epistle.Repositories;
using MongoDB.Driver;

using Object = Epistle.ActivityPub.Object;

namespace Epistle;

public class MongoDbRepository : IMongoDbRepository
{
    private readonly IMongoDatabase _db;

    public MongoDbRepository(IConfiguration configuration)
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

    public IQueryable<Object> FindAllObjects()
    {
        return Objects.AsQueryable();
    }

    public IQueryable<Activity> FindAllActivities()
    {
        return Activities.AsQueryable();
    }

    public IQueryable<Actor> FindAllActors()
    {
        return Actors.AsQueryable();
    }

    private IMongoCollection<Object> Objects => GetCollection<Object>("objects");

    private IMongoCollection<Activity> Activities => GetCollection<Activity>("activities");

    private IMongoCollection<Actor> Actors => GetCollection<Actor>("actor");

    private IMongoCollection<T> GetCollection<T>(string name) =>
        _db.GetCollection<T>(name)
            ?? throw new InvalidOperationException($"unable to find {name} collection");
}
