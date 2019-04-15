using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IThreadRepository _threadRepository;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository, IThreadRepository threadRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _threadRepository = threadRepository;
            _mapper = mapper;
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, UserStatus status)
        {
            return await _userRepository.UpdateChatStatusAsync(userId, status.ToString());
        }
    }
}