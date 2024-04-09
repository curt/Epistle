using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Epistle;

// See: https://makolyte.com/csharp-serialize-to-json-in-alphabetical-order/
public class AlphabeticalOrderJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);
        int order = 0;

        foreach (var property in jsonTypeInfo.Properties.OrderBy(p => p.Name))
        {
            property.Order = property.Name switch
            {
                "@context" => -3,
                "id" => -2,
                "type" => -1,
                _ => order++
            };
        }

        return jsonTypeInfo;
    }
}
