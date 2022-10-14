using MongoDB.Bson.Serialization.Attributes;

namespace WSDISCOUNT.Models
{
    public class ObjetDescuento
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Console { get; set; }
        public int Discount { get; set; }
        public int Maximum_price { get; set; }
        public int Minimal_price { get; set; }

    }
}
