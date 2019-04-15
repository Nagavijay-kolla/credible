using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CBH.Chat.Domain.ChatDomains.Entity
{
    public class EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    }
}