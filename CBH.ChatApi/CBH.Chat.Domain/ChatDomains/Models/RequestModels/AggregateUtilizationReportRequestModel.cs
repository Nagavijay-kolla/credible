using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBH.Chat.Domain.ChatDomains.Models.RequestModels
{
    public class AggregateUtilizationReportRequestModel
    {
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("employee_ids")]
        public List<int> EmployeeIds { get; set; }
    }
}
