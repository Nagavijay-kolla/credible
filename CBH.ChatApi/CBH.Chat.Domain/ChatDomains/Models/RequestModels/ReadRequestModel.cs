using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class ReadRequestModel
    {
        public ReadRequestModel()
        {
            MessageIds = new List<Guid>();
        }

        [JsonProperty("message_id")]
        public List<Guid> MessageIds { get; set; }
    }
}
