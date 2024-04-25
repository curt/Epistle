using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Epistle.Repositories;

public class EnumerableTripleBsonSerializer : SerializerBase<IEnumerableTriple>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IEnumerableTriple value)
    {
        context.Writer.WriteStartArray();

        foreach (var t in value)
        {
            if (t.Base is null)
                throw new NotSupportedException("cannot serialize without base type");

            switch (t.EntityType)
            {
                case TripleEnum.Uri:
                    context.Writer.WriteString(t.Uri.ToString());
                    break;
                case TripleEnum.Object:
                    BsonSerializer.Serialize(context.Writer, (Object)t.Base);
                    break;
                case TripleEnum.Link:
                    BsonSerializer.Serialize(context.Writer, (Link)t.Base);
                    break;
                default:
                    throw new NotSupportedException($"cannot serialize EntityType '{t.EntityType}'");
            }
        }

        context.Writer.WriteEndArray();
    }

    public override IEnumerableTriple Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var list = new EnumerableTriple();

        switch (context.Reader.GetCurrentBsonType())
        {
            case BsonType.Array:
                // var list = new EnumerableTriple();
                context.Reader.ReadStartArray();

                while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
                {
                    DeserializeDocument(list, context);
                }

                context.Reader.ReadEndArray();
                break;
            case BsonType.Document:
                DeserializeDocument(list, context);
                break;
            case BsonType.String:
                list.Add(new Triple(new Uri(context.Reader.ReadString())));
                break;
            case BsonType.Null:
                context.Reader.ReadNull();
                break;
            default:
                throw new NotImplementedException();
        }

        return list;
    }

    private static void DeserializeDocument(EnumerableTriple list, BsonDeserializationContext context)
    {
        if (context.Reader.GetCurrentBsonType() == BsonType.String)
        {
            list.Add(new Triple(new Uri(context.Reader.ReadString())));
        }
        else
        {
            var bookmark = context.Reader.GetBookmark();
            var core = BsonSerializer.Deserialize<Core>(context.Reader);
            context.Reader.ReturnToBookmark(bookmark);

            if (Actor.ActorTypes.Contains(core.Type))
            {
                list.Add(new Triple(BsonSerializer.Deserialize<Actor>(context.Reader)));
            }
            else if (Activity.ActivityTypes.Contains(core.Type))
            {
                list.Add(new Triple(BsonSerializer.Deserialize<Activity>(context.Reader)));
            }
            else if (Link.LinkTypes.Contains(core.Type))
            {
                list.Add(new Triple(BsonSerializer.Deserialize<Link>(context.Reader)));
            }
            else if (Object.ObjectTypes.Contains(core.Type))
            {
                list.Add(new Triple(BsonSerializer.Deserialize<Object>(context.Reader)));
            }
            else
            {
                throw new NotSupportedException($"cannot deserialize Type '{core.Type}'");
            }
        }
    }
}