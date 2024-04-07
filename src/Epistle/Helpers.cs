using Epistle.ActivityPub;
using Microsoft.AspNetCore.Http.Extensions;

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
        var builder = new UriBuilder(uri)
        {
            Scheme = endpoint.Scheme,
            Host = endpoint.Host,
            Port = endpoint.Port
        };
        return builder.Uri;
    }

    public static ActivityPub.Object Publicize(this ActivityPub.Object obj, string endpoint)
    {
        return obj.Publicize(new Uri(endpoint));
    }

    public static ActivityPub.Object Publicize(this ActivityPub.Object obj, Uri endpoint)
    {
        obj.Id = obj.Id?.ToPublicUri(endpoint);
        return obj;
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
