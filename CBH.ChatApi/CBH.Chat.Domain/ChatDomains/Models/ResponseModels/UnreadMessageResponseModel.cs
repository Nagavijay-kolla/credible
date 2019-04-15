using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class UnreadMessageResponseModel
    {
        [JsonProperty("chatUserId")]
        public int ChatUserId { get; set; }

        [JsonProperty("unread_messages_count")]
        public int UnreadMessagesCount { get; set; }
    }
}