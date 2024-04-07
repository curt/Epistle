namespace Epistle;

public static class Helpers
{
    public static Uri ToInternalUri(this Uri uri)
    {
        var builder = new UriBuilder("http", "internal")
        {
            Path = uri.AbsolutePath
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
}