using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;

namespace CBH.Chat.Interfaces.Business
{
    public interface IThreadManager
    {
        Task<ThreadWithMessagesResponseModel> GetThreadById(Guid threadId);
        Task<ThreadWithMessagesResponseModel> SearchOrCreateThreadAsync(int userId, string participantId, ThreadType type);
        Task<ThreadWithMessagesResponseModel> AddNewMessageAsync(NewMessageRequestModel newMessage);
        Task<ThreadWithMessagesResponseModel> SearchOrCreateBroadcastThreadAsync();
        Task ArchiveThreadyByIdAsync(int userId, Guid threadId, bool status);
        Task<IEnumerable<ThreadWithContactsResponseModel>> GetThreadsByUserId(int userId, int limit, FetchThreadType fetchType);
        Task SetReadStatusAsync(int userId, IList<Guid> messages);
		Task<IEnumerable<ChatLogResponse>> GetAllThreadsAsync();
    }
}