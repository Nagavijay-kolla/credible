using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class GroupWithUsersResponseModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_userid")]
        public int CreatedUserId { get; set; }

        [JsonProperty("created_username")]
        public string CreatedUsername { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("members")]
        public IList<UserContactResponseModel> Members { get; set; }
    }
}