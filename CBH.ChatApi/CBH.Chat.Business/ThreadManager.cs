using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Infrastructure.Chat.Exception;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business
{
    public class ThreadManager : IThreadManager
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IThreadRepository _threadRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILogRepository _logRepository;

        private readonly IMapper _mapper;

        public ThreadManager(IPartnerRepository partnerRepository, IThreadRepository threadRepository, IMessageRepository messageRepository, IGroupRepository groupRepository,
                                IMapper mapper, IContactRepository contactRepository, ITeamRepository teamRepository, ILogRepository logRepository)
        {
            _partnerRepository = partnerRepository;
            _threadRepository = threadRepository;
            _messageRepository = messageRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
            _contactRepository = contactRepository;
            _teamRepository = teamRepository;
            _logRepository = logRepository;
        }

        public async Task<ThreadWithMessagesResponseModel> AddNewMessageAsync(NewMessageRequestModel newMessage)
        {
             DateTime ?readAt;
            LogEntity logEntity = new LogEntity();
            var newMessageEntity = _mapper.Map<NewMessageRequestModel, MessageEntity>(newMessage);

            var thread = await _threadRepository.GetAsync(newMessageEntity.ThreadId);

            if (thread == null)
            {
                throw new InvalidRequestException();
            }
            thread.ArchivedBy = new List<int>(); 
            await _threadRepository.UpdateAsync(thread);

            newMessageEntity.ReadBy.Add(newMessage.FromUserId);
            if (newMessageEntity.ReadBy.Count > 0)
            {
                readAt = DateTime.UtcNow;
            }
            else
            {
                readAt = null;
            }
            newMessageEntity.IsImportant = newMessage.IsImportant;
            newMessageEntity.FromUserName = newMessage.FromUserName;
            // Add message and Get all messages
            var messages = _mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(await _messageRepository.AddAndGetAsync(newMessageEntity)).ToList();
            
            // update thread modified time
            thread.ModifiedAt = DateTime.UtcNow;
            
            await _threadRepository.UpdateAsync(thread);

            var participantsDetails = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(thread.Participants));
            
            var threadWithMessages = _mapper.Map(messages, _mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(thread, options => options.Items[MappingConstants.ThreadRequestUserId] = newMessage.FromUserId));

            await LogMessage(thread, messages.OrderByDescending(x => x.CreatedAt).First(),readAt);

            return _mapper.Map(participantsDetails, threadWithMessages);
        }

        private async Task LogMessage(ThreadEntity thread, MessageResponseModel message,DateTime ?readAt)
        {
            var log = _mapper.Map<LogEntity>(thread);
            log = _mapper.Map(message, log, options => options.Items[MappingConstants.RecipientUserId] = thread.Type == ThreadType.User ? message.FromUserId : 0);
            log.Id = new Guid();
            log.RecepientName = thread.Name;
            log.SenderName = message.FromUserName;
            log.CreatedAt = message.CreatedAt;
            log.IsArchived = thread.ArchivedBy.Count == 0 ? false : true;
            if (message.ReadBy.Contains(log.SenderId) && message.ReadBy.Count == 1)
                log.IsRead = false;
            else if (message.ReadBy.Count > 1)
                log.IsRead = true;
            if (log.IsArchived == false) log.ArchivedAt = null;
            else log.ArchivedAt = thread.ModifiedAt;
            if (log.IsRead == false) log.ReadAt = null;
            else log.ReadAt = readAt;
            
            switch (thread.Type)
            {
                case ThreadType.User:
                    log.RecipientId = thread.Participants[1];
                    break;
                case ThreadType.Group:
                    log.RecipientId = 0;
                    break;

                case ThreadType.Team:
                    log.RecipientId = thread.TeamId;
                    break;
                default:
                    log.RecipientId = 0 ;
                    break;
                    
            }
            await _logRepository.CreateAsync(log);
        }

        public async Task<ThreadWithMessagesResponseModel> SearchOrCreateThreadAsync(int userId, string participant, ThreadType threadType)
        {
            ThreadEntity thread = null;

            switch (threadType)
            {
                case ThreadType.User:
                    {
                        if (!int.TryParse(participant, out var participantId))
                        {
                            throw new InvalidRequestException(nameof(participant));
                        }

                        thread = await GetOrCreateUserThreadAsync(userId, participantId);
                        break;
                    }
                case ThreadType.Group:
                    {
                        if (!Guid.TryParse(participant, out var participantGuId))
                        {
                            throw new InvalidRequestException(nameof(participant));
                        }
                        thread = await GetOrCreateGroupThreadAsync(participantGuId);
                        break;
                    }
                case ThreadType.Team:
                    {
                        if (!Guid.TryParse(participant, out var participantId))
                        {
                            throw new InvalidRequestException(nameof(participant));
                        }
                        thread = await GetOrCreateTeamThreadAsync(participantId);
                        break;
                    }
            }

            if (thread == null)
            {
                return new ThreadWithMessagesResponseModel();
            }

            var participantDetails = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(thread.Participants));
            var messages = _mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(await _messageRepository.GetByThreadIdAsync(thread.Id));
            var threadWithMessages = _mapper.Map(messages, _mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(thread, options => options.Items[MappingConstants.ThreadRequestUserId] = userId));

            return _mapper.Map(participantDetails, threadWithMessages);
        }

        private async Task<ThreadEntity> GetOrCreateTeamThreadAsync(Guid participantId)
        {
            var teamDetails = await _teamRepository.GetAsync(participantId);

            if (teamDetails == null)
            {
                throw new InvalidRequestException(ErrorConstants.InvalidParticipantId);
            }

            var teamMembers = _partnerRepository.GetTeamMembers(teamDetails.TeamId);
            var teamParticipants = (await _contactRepository.GetChatUserDetailsFromUserIdsAsync(teamMembers)).Select(x => x.ChatUserId).ToList();
            var thread = await _threadRepository.GetAsync(participantId);

            if (thread == null)
            {
                return await _threadRepository.CreateAsync(new ThreadEntity
                {
                    Id = participantId,
                    Name = teamDetails.Name,
                    Participants = teamParticipants,
                    Type = ThreadType.Team,
                    ArchivedBy = new List<int>(),
                    CreatedAt = DateTime.UtcNow,
                    GroupId = participantId,
                    TeamId = teamDetails.TeamId
                });
            }

            thread.Participants = teamParticipants;
            await _threadRepository.UpdateAsync(thread);
            return thread;
        }

        private async Task<ThreadEntity> GetOrCreateGroupThreadAsync(Guid groupGuid)
        {
            var groupDetails = await _groupRepository.GetAsync(groupGuid);
            if (groupDetails == null)
            {
                throw new InvalidRequestException(ErrorConstants.InvalidParticipantId);
            }

            var thread = await _threadRepository.SearchByGroupIdAsync(groupGuid);

            if (thread != null)
            {
                return thread;
            }

            return await _threadRepository.CreateAsync(new ThreadEntity
            {
                Id = groupGuid,
                Name = groupDetails.Name,
                Participants = groupDetails?.Members,
                Type = ThreadType.Group,
                ArchivedBy = new List<int>(),
                CreatedAt = DateTime.UtcNow,
                GroupId = groupGuid
            });
        }

        private async Task<ThreadEntity> GetOrCreateBroadcastThreadAsync()
        {
            var thread = await _threadRepository.GetByTypeAsync(ThreadType.Broadcast);
            if (thread != null)
            {
                return thread;
            }

            return await _threadRepository.CreateAsync(new ThreadEntity
            {
                Id = Guid.NewGuid(),
                Type = ThreadType.Broadcast
            });

        }

        private async Task<ThreadEntity> GetOrCreateUserThreadAsync(int userId, int participantId)
        {
            var participantDetails = await _contactRepository.GetChatUserDetailAsync(participantId);

            if (participantDetails == null)
            {
                throw new InvalidRequestException(ErrorConstants.InvalidParticipantId);
            }

            var thread = await _threadRepository.SearchByParticipantIdsAsync(userId, participantId);

            if (thread != null)
            {
                return thread;
            }

            return await _threadRepository.CreateAsync(new ThreadEntity
            {
                Id = Guid.NewGuid(),
                Participants = new List<int>
                {
                    userId,
                    participantId
                },
                Name = participantDetails.Name,
                Type = ThreadType.User,
                ArchivedBy = new List<int>(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            });
        }

        public async Task ArchiveThreadyByIdAsync(int userId, Guid threadId, bool status)
        {
            var thread = await _threadRepository.GetAsync(threadId);

            if (thread == null)
            {
                throw new InvalidRequestException();
            }
            if (thread.ArchivedBy == null)
            {
                thread.ArchivedBy = new List<int>();
            }

            if (status)
            {
                if (thread.ArchivedBy?.Contains(userId) == false)
                {
                    thread.ArchivedBy.Add(userId);
                    await _threadRepository.UpdateAsync(thread);
                }
            }
            else
            {
                if (thread.ArchivedBy?.Contains(userId) == true)
                {
                    thread.ArchivedBy.Remove(userId);
                    await _threadRepository.UpdateAsync(thread);
                }
            }
        }

        private static ThreadWithContactsResponseModel ZipContactsWithThread(ThreadWithContactsResponseModel thread, IEnumerable<UserContactResponseModel> contacts)
        {
            thread.Participants = contacts.ToList();
            return thread;
        }

        public async Task<ThreadWithMessagesResponseModel> GetThreadById(Guid threadId)
        {
            var thread = await _threadRepository.GetAsync(threadId);

            if (thread == null)
            {
                throw new InvalidRequestException(ErrorConstants.InvalidParticipantId);
            }

            var participantDetails = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(thread.Participants));

            var messages = _mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(await _messageRepository.GetByThreadIdAsync(thread.Id));

            var threadWithMessages = _mapper.Map(messages, _mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(thread));

            return _mapper.Map(participantDetails, threadWithMessages);
        }

        public async Task<ThreadWithMessagesResponseModel> SearchOrCreateBroadcastThreadAsync()
        {
            var thread = await GetOrCreateBroadcastThreadAsync();
            var participantDetails = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(thread.Participants));

            var messages = _mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(await _messageRepository.GetByThreadIdAsync(thread.Id));

            var threadWithMessages = _mapper.Map(messages, _mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(thread));

            return _mapper.Map(participantDetails, threadWithMessages);
        }

        public async Task<IEnumerable<ThreadWithContactsResponseModel>> GetThreadsByUserId(int userId, int limit, FetchThreadType fetchType)
        {
            IList<ThreadEntity> threads;
            switch (fetchType)
            {
                case FetchThreadType.All:
                    threads = (await _threadRepository.SearchByParticipantIdAsync(userId))?.ToList();
                    break;

                case FetchThreadType.Archived:
                    threads = (await _threadRepository.SearchByParticipantIdAsync(userId))?.Where(x => x.ArchivedBy?.Contains(userId) == true)?.ToList();
                    break;

                case FetchThreadType.UnArchived:
                    threads = (await _threadRepository.SearchByParticipantIdAsync(userId))?.Where(x => x.ArchivedBy == null || x.ArchivedBy?.Contains(userId) == false)?.ToList();
                    break;

                default:
                    throw new InvalidRequestException();
            }

            if (threads == null)
            {
                return _mapper.Map<IEnumerable<ThreadEntity>, IEnumerable<ThreadWithContactsResponseModel>>(null);
            }

            var participants = (await Task.WhenAll(threads.Select(x => _contactRepository.GetChatUserDetailsAsync(x.Participants))))
                .Where(x => x != null)
                .ToList();
            var messages = (await Task.WhenAll(threads.Select(x => _messageRepository.GetByThreadIdAsync(x.Id))))
                .SelectMany(x => x)
                .Where(x => x != null).OrderBy(x => x.CreatedAt);

            var participantDetails = _mapper.Map<IEnumerable<IEnumerable<ChatUser>>, IEnumerable<IEnumerable<UserContactResponseModel>>>(participants);

            var threadWithContactsResponseModels = limit > 0
                ? _mapper.Map<IEnumerable<ThreadEntity>, IEnumerable<ThreadWithContactsResponseModel>>(threads.Take(limit), options => options.Items[MappingConstants.ThreadRequestUserId] = userId).Zip(participantDetails.Take(limit), ZipContactsWithThread)
                : _mapper.Map<IEnumerable<ThreadEntity>, IEnumerable<ThreadWithContactsResponseModel>>(threads, options => options.Items[MappingConstants.ThreadRequestUserId] = userId).Zip(participantDetails, ZipContactsWithThread);


            if (fetchType == FetchThreadType.Archived)
            {
                return threadWithContactsResponseModels.Select(threadResponse =>
                {
                    threadResponse.UnreadMessageCount = threadResponse.Participants.Select(p => new UnreadMessageResponseModel
                    {
                        ChatUserId = p.ChatUserId,
                        UnreadMessagesCount = messages.Where(m => m.ThreadId == threadResponse.Id).Count(m => m.ReadBy?.Contains(p.ChatUserId) != true)
                    });
                    return threadResponse;
                }).OrderByDescending(x => x.ModifiedAt);

            }
            return threadWithContactsResponseModels.Select(threadResponse =>
            {
                threadResponse.UnreadMessageCount = threadResponse.Participants.Select(p => new UnreadMessageResponseModel
                {
                    ChatUserId = p.ChatUserId,
                    UnreadMessagesCount = messages.Where(m => m.ThreadId == threadResponse.Id).Count(m => m.ReadBy?.Contains(p.ChatUserId) != true)
                });
                return threadResponse;
            }).OrderBy(x => x.ModifiedAt);
        }

        public async Task SetReadStatusAsync(int userId, IList<Guid> messageIds)
        {
            foreach (var messageId in messageIds)
            {
                var message = await _messageRepository.GetAsync(messageId);

                if (message?.ReadBy?.Contains(userId) != false)
                {
                    continue;
                }

                message.ReadBy.Add(userId);
                await _messageRepository.UpdateAsync(message);
            }
        }

        public async Task<IEnumerable<ChatLogResponse>> GetAllThreadsAsync()
        {
            var threads = await _threadRepository.GetAllThreadsAsync();
            var threadList = new List<ThreadWithMessagesResponseModel>();

            foreach (var thread in threads)
            {
                var participantDetails = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(thread.Participants));

                var messages = _mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(await _messageRepository.GetByThreadIdAsync(thread.Id)).OrderBy(x => x.CreatedAt);

                var threadWithMessages = _mapper.Map(messages, _mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(thread));

                threadList.Add(_mapper.Map(participantDetails, threadWithMessages));
            }

            return _mapper.Map<IEnumerable<ThreadWithMessagesResponseModel>, IEnumerable<ChatLogResponse>>(threadList);
        }
    }
}