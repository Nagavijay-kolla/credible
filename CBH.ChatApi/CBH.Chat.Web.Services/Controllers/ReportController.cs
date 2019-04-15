using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogManager _logManager;

        public ReportController(ILogManager logManager)
        {
            _logManager = logManager;
        }

        [HttpGet("chatLogs/{userId}/{partnerId}")]
        public async Task<IEnumerable<ChatLogResponse>> ChatLogs([FromRoute] int userId, [FromRoute] int partnerId)
        {
            return await _logManager.ChatLogAsync(userId, partnerId);
        }

        [HttpGet("adminChatLogs/{partnerId}")]
        public async Task<IEnumerable<ChatLogResponse>> AdminChatLogs([FromRoute] int partnerId)
        {
            return await _logManager.AdminChatLogAsync(partnerId);
        }

        [HttpPost("adminSentMessages/{partnerId}")]
        public async Task<IEnumerable<SentMessageReportResponseModel>> AdminSentMessages([FromRoute] int partnerId, [FromBody]SentMessageReportRequestModel filter)
        {
            return await _logManager.AdminSentMessagesAsync(partnerId, filter);
        }

        [HttpPost("adminChatUtilizationReport/{partnerId}")]
        public async Task<IEnumerable<AggregateUtilizationReportResponseModel>> AdminAggregateUtilization([FromRoute] int partnerId, [FromBody]AggregateUtilizationReportRequestModel filter)
        {
            return await _logManager.AdminAggregateUtilizationAsync(partnerId, filter);
        }
    }
}