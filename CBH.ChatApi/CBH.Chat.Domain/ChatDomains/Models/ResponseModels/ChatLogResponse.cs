using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class ChatLogResponse
    {
        [JsonProperty("thread_id")]
        public Guid ThreadId { get; set; }

        [JsonProperty("message_id")]
        public Guid MessageId { get; set; }

        [JsonProperty("recepient_id")]
        public string RecipientId { get; set; }

        [JsonProperty("recipient_name")]
        public string RecipientName { get; set; }

        [JsonProperty("sender_id")]
        public int SenderId { get; set; }

        [JsonProperty("sender_name")]
        public string SenderName { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("participants")]
        public IList<UserContactResponseModel> Participants { get; set; }

        [JsonProperty("is_read")]
        public bool IsRead { get; set; }

        [JsonProperty("read_at")]
        public DateTime? ReadAt { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("archived_at")]
        public DateTime? ArchivedAt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("is_important")]
        public bool IsImportant { get; set; }
    }
}