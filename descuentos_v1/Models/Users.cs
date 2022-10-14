using MongoDB.Bson.Serialization.Attributes;

namespace WSDISCOUNT.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
