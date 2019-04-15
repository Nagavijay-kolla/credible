using System.Threading.Tasks;
using CBH.Chat.Domain.Core.Dtos;

namespace CBH.Chat.Interfaces.Business
{
    public interface IUserConfigurationManager
    {
        Task<UserConfigurationDto> GetUserChatConfigurationAsync(int userId);
    }
}