using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PersonalCrm
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }

        public string Name { get; init; }

        public string Email { get; init; }

        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public DateTime UpdatedAt { get; init; } = DateTime.Now;

        [HotChocolate.GraphQLIgnore]
        public string Salt { get; init; }

        [HotChocolate.GraphQLIgnore]
        public string Hash { get; init; }
    }
}