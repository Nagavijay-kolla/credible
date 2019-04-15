using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Enumerations;

namespace CBH.Chat.Interfaces.Business
{
    public interface IUserManager
    {
        Task<bool> UpdateUserStatusAsync(int userId, UserStatus status);
    }
}