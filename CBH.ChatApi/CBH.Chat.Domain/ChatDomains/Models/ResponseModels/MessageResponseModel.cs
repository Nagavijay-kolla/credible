using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class MessageResponseModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("thread_id")]
        public Guid ThreadId { get; set; }

        [JsonProperty("from_username")]
        public string FromUserName { get; set; }

        [JsonProperty("read_by")]
        public IList<int> ReadBy { get; set; }

        [JsonProperty("from_userid")]
        public int FromUserId { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("is_important")]
        public bool IsImportant { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}