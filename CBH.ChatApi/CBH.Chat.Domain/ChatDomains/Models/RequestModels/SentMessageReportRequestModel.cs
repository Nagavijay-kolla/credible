using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class SentMessageReportRequestModel
    {

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("sender_ids")]
        public List<int> SenderIds { get; set; }

        [JsonProperty("recepient_ids")]
        public List<int> RecepientIds { get; set; }

        [JsonProperty("is_important")]
        public bool ?IsImportant { get; set; }
    }
}
