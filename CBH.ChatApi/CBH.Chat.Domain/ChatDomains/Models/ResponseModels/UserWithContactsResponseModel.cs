using System;
using System.Collections.Generic;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class UserWithContactsResponseModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("chat_user_id")]
        public int ChatUserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_chat_enabled")]
        public bool IsChatEnabled { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("status")]
        public UserStatus Status { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("contacts")]
        public IList<UserContactResponseModel> Contacts { get; set; }

        public UserWithContactsResponseModel()
        {
            Contacts = new List<UserContactResponseModel>();
        }
    }
}