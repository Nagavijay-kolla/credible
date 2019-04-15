using System.Linq;
using System.Threading.Tasks;
using CBH.Chat.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.ChatRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IChatUserDbContext _chatUserDbContext;

        public UserRepository(IChatDbClient dbClient, IChatUserDbContext chatUserDbContext)
        {
            _chatUserDbContext = chatUserDbContext;
        }

        public async Task<bool> UpdateChatStatusAsync(int chatUserId, string status)
        {
            var user = await _chatUserDbContext.ChatUsers.Where(x => x.ChatUserId == chatUserId).SingleOrDefaultAsync();
            if (user != null)
            {
                user.Status = status;
                _chatUserDbContext.ChatUsers.Attach(user);
                return _chatUserDbContext.SaveChanges() > 0;
            }
            return false;
        }
    }
}