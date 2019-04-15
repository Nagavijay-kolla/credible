using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Domain.Core.Entities;
using System;
using System.Collections.Generic;

namespace CBH.Chat.Business.Tests.DataHelpers
{
    public static class EntityModellers
    {
        private static readonly Random Rnd = new Random();

        private const string SendBroadcast = "SendBroadcast";
        private const string AllTeams = "ChatAllTeams";
        private const string EmployeeMessageLogViewAll = "EmployeeMessageLogViewAll";
        private const string AppointmentArrivalMessage = "AppointmentArrivalMessage";
        private const string SendEmployeeMessage = "SendEmpMessage";

        public static ChatUser CreateChatUserEntity()
        {
            return new ChatUser
            {
                ChatUserId = Rnd.Next(111, 1000),
                UserId = Rnd.Next(111, 1000),
                PartnerId = Rnd.Next(111, 1000),
                FirstName = "FName",
                Status = "Available",
                LastName = "LName",
                UserRole = "Admin"
            };
        }

        public static GroupEntity CreateGroupEntity()
        {
            return new GroupEntity
            {
                Id = Guid.NewGuid(),
                Name = "User1",
                CreatedUsername = "User2",
                CreatedAt = DateTime.Now,
                CreatedUserId = 1,
                Members = new List<int>()
            };
        }

        public static NewGroupRequestModel CreateNewGroupRequestModel()
        {
            return new NewGroupRequestModel
            {
                Name = "Group1",
                CreatedUserId = Rnd.Next(111, 1000),
                CreatedUserName = "User1"
            };
        }

        public static TeamEntity CreateTeamEntity()
        {
            return new TeamEntity
            {
                Id = Guid.NewGuid(),
                Name = "UserName1",
                CreatedAt = DateTime.Now,
                Members = new List<int>(),
                TeamId = Rnd.Next(111, 1000)
            };
        }

        public static ThreadEntity CreateThreadEntity()
        {
            return new ThreadEntity
            {
                Id = Guid.NewGuid(),
                Participants = new List<int>(),
                Name = "Thread1",
                Type = ThreadType.User,
                ArchivedBy = new List<int>(),
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                GroupId = Guid.NewGuid()
            };
        }

        public static MessageEntity CreateMessageEntity()
        {
            return new MessageEntity
            {
                Id = Guid.NewGuid(),
                ThreadId = Guid.NewGuid(),
                FromUserName = "User1",
                FromUserId = Rnd.Next(111, 1000),
                Content = "Message1",
                CreatedAt = DateTime.Now
            };
        }

        public static NewMessageRequestModel CreateNewMessageRequestModel()
        {
            return new NewMessageRequestModel
            {
                ThreadId = Guid.NewGuid(),
                Content = "Message1",
                FromUserName = "User1",
                FromUserId = Rnd.Next(111, 1000)
            };
        }

        public static User CreateUserEntity(int userId)
        {
            return new User
            {
                Id = (short)userId,
                FirstName = "FName",
                LastName = "LName",
                UserName = "LName FName",
                UserRole = "Admin",
                IsEnableChat = true,
                IsAdmin = true,
                IsHighImportanceEnable = true,
                Deleted = true,
                Status = "Available",
                IsEmployee = true,
                ProfileCode = "ADMIN"
            };
        }

        public static PartnerDto CreatePartnerEntity()
        {
            return new PartnerDto
            {
                PartnerId = Rnd.Next(111, 1000),
                PartnerName = "Partner1"
            };
        }

        public static string[] GetUserRights()
        {
            return new[]
{
                SendBroadcast,
                AllTeams,
                EmployeeMessageLogViewAll,
                AppointmentArrivalMessage,
                SendEmployeeMessage
            };
        }

        public static SentMessageReportRequestModel GetSentMessageReportRequestModel()
        {
            return new SentMessageReportRequestModel
            {
                SenderIds = new List<int>() { 1, 2, 3 },
                RecepientIds = new List<int>() { 1, 2, 3 },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                IsImportant = true
            };
        }

        public static AggregateUtilizationReportRequestModel GetAggregateUtilizationReportRequestModel()
        {
            return new AggregateUtilizationReportRequestModel
            {
                EmployeeIds = new List<int>() { 1, 2, 3 },
                StartDate = Convert.ToDateTime("3/2/2019"),
                EndDate = Convert.ToDateTime("3/7/2019"),
            };
        }

        public static LogEntity GetLogEntity()
        {
            return new LogEntity
            {
                RecipientId = 2,
                RecepientName = "User1",
                ThreadId = Guid.NewGuid(),
                SenderId = 1,
                MessageId = Guid.NewGuid(),
                Message = "message",
                CreatedAt = DateTime.Now,
                IsImportant = true,
                IsArchived = true,
                IsRead = true,
                ReadAt = DateTime.Now,
                ArchivedAt = DateTime.Now,
                TeamId = Rnd.Next(111, 1000)
            };
        }

        public static ChatLogResponse GetChatLogResponse()
        {
            return new ChatLogResponse
            {
                ThreadId = Guid.NewGuid(),
                SenderId = Rnd.Next(111, 1000),
                RecipientId = "Id",
                MessageId = Guid.NewGuid(),
                Message = "message",
                CreatedAt = DateTime.Now,
                IsImportant = true,
                IsArchived = true,
                IsRead = true,
                ReadAt = DateTime.Now,
                ArchivedAt = DateTime.Now,
                Type = "user",
                TeamId = Rnd.Next(111, 1000)
            };
        }


        public static ChatUser GetChatUserEntity()
        {
            return new ChatUser
            {
                ChatUserId = 1,
                UserId = 1,
                PartnerId = 2,
                FirstName = "FName",
                Status = "Available",
                LastName = "LName",
                UserRole = "Admin"
            };
        }
        public static LogEntity GetSentLogsEntity()
        {
            return new LogEntity
            {
                RecipientId = 1,
                RecepientName = "User1",
                ThreadId = Guid.NewGuid(),
                SenderId = 1,
                MessageId = Guid.NewGuid(),
                Message = "message",
                CreatedAt = DateTime.Now,
                IsImportant = true,
                IsArchived = true,
                IsRead = true,
                ReadAt = DateTime.Now,
                ArchivedAt = DateTime.Now,
                TeamId = Rnd.Next(111, 1000)
            };

        }

        public static SentMessageReportResponseModel GetSentMessageReportResponseModel()
        {
            return new SentMessageReportResponseModel
            {
                SenderName = "user1",
                RecipientName = "user2",
                DaysUnread = 12,
                IsImportant = true,
                IsRead = false,
                MessageTime = DateTime.Now
            };
        }

        public static AggregateUtilizationReportResponseModel GetAggregateUtilizationReportResponseModel()
        {
            return new AggregateUtilizationReportResponseModel
            {
                EmployeeName = "User1",
                TotalReceivedMessages = 9,
                TotalRepliedMessages = 10,
                TotalSentMessages = 1
            };
        }
}
}


