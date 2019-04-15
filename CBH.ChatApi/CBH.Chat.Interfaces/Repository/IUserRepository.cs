using System.Threading.Tasks;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<bool> UpdateChatStatusAsync(int chatUserId, string status);
    }
}