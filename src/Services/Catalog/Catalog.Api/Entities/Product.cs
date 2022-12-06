using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")] // you can give the column name here if you want to change the property name in the class
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? ImageFile { get; set; }
        public decimal Price { get; set; }


    }
}
