using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CBH.Chat.Domain.ChatDomains.Entity
{
    public class MessageEntity : EntityBase
    {
        [BsonElement("thread_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid ThreadId { get; set; }

        [BsonElement("from_user_name")]
        public string FromUserName { get; set; }

        [BsonElement("from_user_id")]
        public int FromUserId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("is_important")]
        public bool IsImportant { get; set; }

        [BsonElement("read_by")]
        [BsonRepresentation(BsonType.String)]
        public IList<int> ReadBy { get; set; }

        public MessageEntity()
        {
            ReadBy = new List<int>();
        }
    }
}