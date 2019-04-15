using System;
using System.Collections.Generic;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CBH.Chat.Domain.ChatDomains.Entity
{
    public class ThreadEntity : EntityBase
    {
        [BsonElement("participants")]
        public IList<int> Participants { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("type")]
        public ThreadType Type { get; set; }

        [BsonElement("archived_by")]
        public IList<int> ArchivedBy { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("modified_at")]
        public DateTime ModifiedAt { get; set; }

        [BsonElement("group_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid GroupId { get; set; }

        [BsonElement("team_id")]
        public int TeamId { get; set; }

        public ThreadEntity()
        {
            Participants = new List<int>();
            ArchivedBy = new List<int>();
        }
    }
}