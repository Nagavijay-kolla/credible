using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class NewBroadcastMessageRequestModel
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("from_username")]
        public string FromUserName { get; set; }

        [JsonProperty("from_userid")]
        public int FromUserId { get; set; }
    }
}