using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PersonalCrm
{
    public class Reminder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Title { get; init; }
        public string Schedule { get; init; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; init; }
        public User? Owner { get; set; }

    }
}