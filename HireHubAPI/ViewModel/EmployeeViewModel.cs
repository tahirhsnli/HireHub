using MongoDB.Bson.Serialization.Attributes;

namespace HireHubAPI.ViewModel
{
    public class EmployeeViewModel
    {
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
