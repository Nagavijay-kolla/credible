using System;
using System.Threading.Tasks;
using CBH.Chat.Domain.Core.Dtos;

namespace CBH.Chat.Interfaces.Business
{
    public interface IContactManager
    {
        Task<ContactUsersDto> GetContactsAsync(int userId);
        Task<bool> AddContactAsync(int chatUserId, int contactUserId);
        Task<bool> DeleteContactAsync(int chatUserId, int contactUserId);
        Task<ContactUsersDto> GetMyContactsAsync(int chatUserId);
        Task<ContactUsersDto> SearchContactsAsync(int chatUserId, string searchString);
        Task<ContactUsersDto> GetGroupContactsSearch(int chatUserId, Guid groupId);
    }
}