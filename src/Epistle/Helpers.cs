namespace Epistle;

public static class Helpers
{
    public static Uri ToInternalUri(this Uri uri)
    {
        return InternalUri(uri.AbsolutePath);
    }

    public static Uri ToInternalUri(this HttpRequest request)
    {
        return InternalUri(request.Path);
    }

    private static Uri InternalUri(string path)
    {
        var builder = new UriBuilder("http", "internal")
        {
            Path = path
        };

        return builder.Uri;
    }

    public static Uri ToPublicUri(this Uri uri, string endpoint)
    {
        return ToPublicUri(uri, new Uri(endpoint));
    }

    public static Uri ToPublicUri(this Uri uri, Uri endpoint)
    {
        if (uri.Host.Equals("internal", StringComparison.OrdinalIgnoreCase))
        {
            var builder = new UriBuilder(uri)
            {
                Scheme = endpoint.Scheme,
                Host = endpoint.Host,
                Port = endpoint.Port
            };
            return builder.Uri;
        }

        return uri;
    }

    public static IEnumerableTriple ToPublicUri(this IEnumerableTriple triples, Uri endpoint)
    {
        var results = new EnumerableTriple();

        foreach (var triple in triples)
        {
            results.Add(triple.ToPublicUri(endpoint));
        }

        return results;
    }

    public static Triple ToPublicUri(this Triple triple, Uri endpoint)
    {
        switch (triple.EntityType)
        {
            case TripleEnum.Uri:
                return new Triple(((Uri)triple.Base!).ToPublicUri(endpoint));
            case TripleEnum.Link:
                var link = (Link)triple.Base!;
                link.Href = link.Href!.ToPublicUri(endpoint);
                return new Triple(link);
            case TripleEnum.Object:
                return new Triple(((Object)triple.Base!).Publicize(endpoint));
        }

        return triple;
    }

    public static Object Publicize(this Object obj, string endpoint)
    {
        return obj.Publicize(new Uri(endpoint));
    }

    public static Object Publicize(this Object obj, Uri endpoint)
    {
        obj.Id = obj.Id?.ToPublicUri(endpoint);
        obj.AttributedTo = obj.AttributedTo?.ToPublicUri(endpoint);

        return obj;
    }

    public static Actor Publicize(this Actor actor, Uri endpoint)
    {
        actor.Id = actor.Id?.ToPublicUri(endpoint);
        actor.Url = actor.Url?.ToPublicUri(endpoint);

        return actor;
    }

    public static Uri ToEndpoint(this HttpRequest request)
    {
        var builder = new UriBuilder()
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
        };

        if (request.Host.Port is not null)
            builder.Port = (int)request.Host.Port;

        return builder.Uri;
    }
}
