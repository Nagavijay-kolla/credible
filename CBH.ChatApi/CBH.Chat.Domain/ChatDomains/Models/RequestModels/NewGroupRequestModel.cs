using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class NewGroupRequestModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_userid")]
        public int CreatedUserId { get; set; }

        [JsonProperty("created_username")]
        public string CreatedUserName { get; set; }
    }
}