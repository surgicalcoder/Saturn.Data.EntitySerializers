using System.Dynamic;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace GoLive.Saturn.Data.EntitySerializers
{
    public class JObjectSerializer : SerializerBase<JObject>
    {
        ExpandoObjectSerializer seri = new ExpandoObjectSerializer();

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JObject value)
        {
            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            var deserializeObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(serializeObject, new ExpandoObjectConverter());
            seri.Serialize(context, deserializeObject);
        }
    }
}