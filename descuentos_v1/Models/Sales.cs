
using MongoDB.Bson.Serialization.Attributes;

namespace WSDISCOUNT.Models
{
    public class Sales
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Console { get; set; }
        public int Value { get; set; }
        public int Value_Paid_Out { get; set; }
    }
}
