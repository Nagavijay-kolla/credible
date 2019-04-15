using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class SentMessageReportResponseModel
    {
        [JsonProperty("message_time")]
        public DateTime MessageTime { get; set; }

        [JsonProperty("sender_name")]
        public string SenderName { get; set; }

        [JsonProperty("recipient_name")]
        public string RecipientName { get; set; }

        [JsonProperty("days_unread")]
        public int DaysUnread { get; set; }

        [JsonProperty("is_read")]
        public bool IsRead { get; set; }

        [JsonProperty("is_important")]
        public bool IsImportant { get; set; }
    }

}

