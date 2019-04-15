using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBH.Chat.Domain.ChatDomains.Models.ResponseModels
{
    public class AggregateUtilizationReportResponseModel
    {
        [JsonProperty("employee_name")]
        public string EmployeeName { get; set; }

        [JsonProperty("total_sent_messages")]
        public int TotalSentMessages { get; set; }

        [JsonProperty("total_received_messages")]
        public int TotalReceivedMessages { get; set; }

        [JsonProperty("total_replied_messages")]
        public int TotalRepliedMessages { get; set; }

    }
}
