using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBH.Chat.Interfaces.Business
{
    public interface ILogManager
    {
        Task<IEnumerable<ChatLogResponse>> ChatLogAsync(int userid,int partnerId);
        Task<IEnumerable<ChatLogResponse>> AdminChatLogAsync(int partnerID);
        Task<IEnumerable<SentMessageReportResponseModel>> AdminSentMessagesAsync(int partnerId,SentMessageReportRequestModel newMessage);
        Task<IEnumerable<AggregateUtilizationReportResponseModel>> AdminAggregateUtilizationAsync(int partnerId, AggregateUtilizationReportRequestModel newMessage);
    }
}
