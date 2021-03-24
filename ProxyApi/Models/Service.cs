using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ProxyApi.Models
{
    public class Service
    {
        // BsonId define id, and BsonRepresentation convert in string 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string RequestValue { get; set; }

        public string Redirect { get; set; }

        //https://stackoverflow.com/questions/6996399/storing-enums-as-strings-in-mongodb
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public Status Status { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Category_id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string App_id { get; set; }
    }


    public enum Status
    {
        auth_required,
        redirect,
        disable,
        maintenance
    }
}
