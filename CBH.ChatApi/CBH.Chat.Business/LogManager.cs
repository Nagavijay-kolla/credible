using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBH.Chat.Business
{
    public class LogManager : ILogManager
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public LogManager(IContactRepository contactRepository, ILogRepository logRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatLogResponse>> ChatLogAsync(int userId, int partnerId)
        {
            var chatUserId = (await _contactRepository.GetChatUserAsync(userId, partnerId)).ChatUserId;
            var logs = (await _logRepository.GetByuserIdAsync(chatUserId)).ToList();
            return _mapper.Map<IEnumerable<Domain.ChatDomains.Entity.LogEntity>, IEnumerable<ChatLogResponse>>(logs);
        }

        public async Task<IEnumerable<ChatLogResponse>> AdminChatLogAsync(int partnerId)
        {
            var usersIds = _contactRepository.GetChatUsersForPartner(partnerId);
            var logs = (await _logRepository.GetAllAsync()).Where(x => usersIds.Contains(x.SenderId) || usersIds.Contains(x.RecipientId));
            return _mapper.Map<IEnumerable<Domain.ChatDomains.Entity.LogEntity>, IEnumerable<ChatLogResponse>>(logs);
        }

        public async Task<IEnumerable<SentMessageReportResponseModel>> AdminSentMessagesAsync(int partnerId, SentMessageReportRequestModel filter)
        {
            var filterSenders = await _contactRepository.GetChatUsersAsync(filter.SenderIds, partnerId);
            var senderIds = filterSenders.AsQueryable().Select(x => x.ChatUserId).ToList();
            var filterRecepients = await _contactRepository.GetChatUsersAsync(filter.RecepientIds, partnerId);
            var recepientIds = filterRecepients.AsQueryable().Select(x => x.ChatUserId).ToList();

            if (filter.IsImportant != null)
            {
                var logs = (await _logRepository.GetAllAsync()).Where(x => senderIds.Contains(x.SenderId) && recepientIds.Contains(x.RecipientId) && x.IsImportant == filter.IsImportant);
                return _mapper.Map<IEnumerable<Domain.ChatDomains.Entity.LogEntity>, IEnumerable<SentMessageReportResponseModel>>(logs);
            }
            else
            {
                var logs = (await _logRepository.GetAllAsync()).Where(x => senderIds.Contains(x.SenderId) && recepientIds.Contains(x.RecipientId));
                return _mapper.Map<IEnumerable<Domain.ChatDomains.Entity.LogEntity>, IEnumerable<SentMessageReportResponseModel>>(logs);
            }

        }
        public async Task<IEnumerable<AggregateUtilizationReportResponseModel>> AdminAggregateUtilizationAsync(int partnerId, AggregateUtilizationReportRequestModel filter)
        {
            var filteremployees = await _contactRepository.GetChatUsersAsync(filter.EmployeeIds, partnerId);
            var employeeIds = filteremployees.AsQueryable().Select(x => x.ChatUserId).ToList();
            var logs = (await _logRepository.GetByDatesAsync(filter, employeeIds)).ToList();
            var results = new List<AggregateUtilizationReportResponseModel>();

            foreach (int employeeId in employeeIds)
            {
                var item = new AggregateUtilizationReportResponseModel();
                var filterSentLog = logs.Where(x => x.SenderId == employeeId).ToList();
                var filterReceivedLog = logs.Where(x => x.RecipientId == employeeId).ToList();
                if ((filterSentLog == null || filterSentLog.Count == 0) && (filterReceivedLog == null || filterReceivedLog.Count == 0))
                {
                    continue;
                }

                if (filterSentLog == null || filterSentLog.Count == 0)
                {
                    item.EmployeeName = filterReceivedLog[0].RecepientName;
                    item.TotalSentMessages = 0;
                    item.TotalReceivedMessages = filterReceivedLog.Count;
                }
                else if (filterReceivedLog == null || filterReceivedLog.Count == 0)
                {
                    item.EmployeeName = filterSentLog[0].SenderName;
                    item.TotalReceivedMessages = 0;
                    item.TotalSentMessages = filterSentLog.Count;
                }
                else
                {
                    item.EmployeeName = filterSentLog[0].SenderName;
                    item.TotalSentMessages = filterSentLog.Count;
                    item.TotalReceivedMessages = filterReceivedLog.Count;
                }
                results.Add(item);
            }
            return results;
        }
    }
}
