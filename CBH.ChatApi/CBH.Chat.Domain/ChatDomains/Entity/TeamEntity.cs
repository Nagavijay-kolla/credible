using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace CBH.Chat.Domain.ChatDomains.Entity
{
    public class TeamEntity : EntityBase
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("teamid")]
        public int TeamId { get; set; }

        [BsonElement("partnerid")]
        public int PartnerId { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("members")]
        public IList<int> Members { get; set; }

        public TeamEntity()
        {
            Members = new List<int>();
        }
    }
}
