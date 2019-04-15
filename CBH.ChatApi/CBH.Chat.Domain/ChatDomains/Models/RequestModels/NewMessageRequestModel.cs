using System;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class NewMessageRequestModel
    {
        [JsonProperty("thread_id")]
        public Guid ThreadId { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("from_username")]
        public string FromUserName { get; set; }

        [JsonProperty("is_important")]
        public bool IsImportant { get; set; }

        [JsonProperty("from_userid")]
        public int FromUserId { get; set; }
    }
}