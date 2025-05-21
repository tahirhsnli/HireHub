using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HireHub.Models
{

    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("position")]
        public string Position { get; set; }

        [BsonElement("office")]
        public string Office { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("startDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartDate { get; set; }

        [BsonElement("salary")]
        public decimal Salary { get; set; }
    }
}
