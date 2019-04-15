using System;
using System.Collections.Generic;
using System.Linq;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class ThreadWithMessagesResponseModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ThreadType Type { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("modified_at")]
        public DateTime ModifiedAt { get; set; }

        [JsonProperty("group_id")]
        public Guid GroupId { get; set; }

        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("participants")]
        public IList<UserContactResponseModel> Participants { get; set; }

        [JsonProperty("messages")]
        public IList<MessageResponseModel> Messages { get; set; }

        [JsonProperty("unread_messages_count")]
        public IEnumerable<UnreadMessageResponseModel> UnreadMessageCount => Participants?.Select(p => new UnreadMessageResponseModel
        {
            ChatUserId = p.ChatUserId,
            UnreadMessagesCount = Messages?.Count(m => m?.ReadBy?.Contains(p.ChatUserId) != true) ?? 0
        });
    }
}