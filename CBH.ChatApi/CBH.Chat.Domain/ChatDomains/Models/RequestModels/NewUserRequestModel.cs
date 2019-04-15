using CBH.Chat.Domain.ChatDomains.Enumerations;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class NewUserRequestModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("chat_user_id")]
        public int ChatUserId { get; set; }

        [JsonProperty("is_chat_enabled")]
        public bool IsChatEnabled { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("status")]
        public UserStatus Status { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
}