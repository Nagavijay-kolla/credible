using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class TeamResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("team_id")]
        public Guid TeamId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("members")]
        public IList<int> Members { get; set; }
    }
}