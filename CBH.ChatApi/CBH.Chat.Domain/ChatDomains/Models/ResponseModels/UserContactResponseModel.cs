using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class UserContactResponseModel
    {
        [JsonProperty("chatUserId")]
        public int ChatUserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("user_role")]
        public string UserRole { get; set; }
    }
}