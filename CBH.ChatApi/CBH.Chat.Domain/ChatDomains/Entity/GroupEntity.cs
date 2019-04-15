using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace CBH.Chat.Domain.ChatDomains.Entity
{
    public class GroupEntity : EntityBase
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("created_username")]
        public string CreatedUsername { get; set; }

        [BsonElement("created_userid")]
        public int CreatedUserId { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("members")]
        public IList<int> Members { get; set; }

        public GroupEntity()
        {
            Members = new List<int>();
        }
    }
}
