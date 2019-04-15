using CBH.Chat.Domain.ChatDomains.Enumerations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CBH.Chat.Domain.ChatDomains.Entity
{
    public class LogEntity : EntityBase
    {
        [BsonElement("thread_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid ThreadId { get; set; }

        [BsonElement("message_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid MessageId { get; set; }

        [BsonElement("recepient_id")]
        public int RecipientId { get; set; }

        [BsonElement("recepient_name")]
        public string RecepientName { get; set; }

        [BsonElement("sender_id")]
        public int SenderId { get; set; }

        [BsonElement("sender_name")]
        public string SenderName { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }

        [BsonElement("is_important")]
        public bool IsImportant { get; set; }

        [BsonElement("is_read")]
        public bool IsRead { get; set; }

        [BsonElement("read_at")]
        public DateTime? ReadAt { get; set; }

        [BsonElement("is_archived")]
        public bool IsArchived { get; set; }

        [BsonElement("archived_at")]
        public DateTime? ArchivedAt { get; set; }

        [BsonElement("type")]
        public ThreadType Type { get; set; }

        [BsonElement("team_id")]
        public int TeamId { get; set; }

        [BsonElement("group_id")]
        public Guid GroupId { get; set; }
    }
}
