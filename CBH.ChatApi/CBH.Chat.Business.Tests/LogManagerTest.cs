using AutoMapper;
using CBH.Chat.Business.Tests.DataHelpers;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;
using KellermanSoftware.CompareNetObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CBH.Chat.Business.Tests
{
    public class LogManagerTest
    {
        private readonly Random _rnd = new Random();

        [Fact]
        public async Task GetChatLogAsyncTest_WhenUserId_ReturnsChatLogs()
        {
            var mockLogRepository = new Mock<ILogRepository>();
            var mockContactRepository = new Mock<IContactRepository>();

            var mockLogManager = new Mock<ILogManager>();
            var mapper = new Mock<IMapper>();
            var chatUser = EntityModellers.CreateChatUserEntity();

            var chatUserList = new List<ChatUser> { chatUser };
            var userId = _rnd.Next(111, 1000);
            var partnerId = _rnd.Next(111, 1000);
            var senderId = _rnd.Next(111, 1000);
            var teamId = _rnd.Next(111, 1000);
            var log = EntityModellers.GetLogEntity();
            var logs = new List<LogEntity> { log };
            var chatResponse = EntityModellers.GetChatLogResponse();
            var expected = new List<ChatLogResponse> { chatResponse };
            mockContactRepository.Setup(x => x.GetChatUserAsync(userId, partnerId)).ReturnsAsync(chatUser);
            mockLogRepository.Setup(x => x.GetByuserIdAsync(chatUser.ChatUserId)).ReturnsAsync(logs);
            mapper.Setup(x => x.Map<IEnumerable<LogEntity>, IEnumerable<ChatLogResponse>>(logs)).Returns(expected);

            var logManager = new LogManager(mockContactRepository.Object, mockLogRepository.Object, mapper.Object);
            var actual = await logManager.ChatLogAsync(userId, partnerId);
            Assert.Equal(expected, actual, new LogicEqualityComparer<ChatLogResponse>());
            mockLogRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAdminChatLogAsyncTest_WhenPartnerId_ReturnsChatLogs()
        {
            var mockLogRepository = new Mock<ILogRepository>();
            var mockContactRepository = new Mock<IContactRepository>();

            var mockLogManager = new Mock<ILogManager>();
            var mapper = new Mock<IMapper>();

            var chatUserList = new List<int> { 1, 2, 3 };
            var userId = _rnd.Next(111, 1000);
            var partnerId = _rnd.Next(111, 1000);
            var senderId = _rnd.Next(111, 1000);
            var teamId = _rnd.Next(111, 1000);
            var log = EntityModellers.GetLogEntity();
            var logs = new List<LogEntity> { log };
            var chatResponse = EntityModellers.GetChatLogResponse();
            var expected = new List<ChatLogResponse> { chatResponse };
            mockContactRepository.Setup(x => x.GetChatUsersForPartner(partnerId)).Returns(chatUserList);
            mockLogRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(logs);
            mapper.Setup(x => x.Map<IEnumerable<LogEntity>, IEnumerable<ChatLogResponse>>(logs)).Returns(expected);

            var logManager = new LogManager(mockContactRepository.Object, mockLogRepository.Object, mapper.Object);
            var actual = await logManager.AdminChatLogAsync(partnerId);
            Assert.Equal(expected, actual, new LogicEqualityComparer<ChatLogResponse>());
            mockLogRepository.VerifyAll();
            mockContactRepository.VerifyAll();
        }

        [Fact]
        public async Task AdminSentMessagesAsyncTest_WhenPartnerIdandFilter_ReturnsSentMessageReport()
        {
            var mockLogRepository = new Mock<ILogRepository>();
            var mockContactRepository = new Mock<IContactRepository>();

            var mockLogManager = new Mock<ILogManager>();
            var mapper = new Mock<IMapper>();

            var chatUser = EntityModellers.GetChatUserEntity();
            var chatUserList = new List<ChatUser> { chatUser };
            var partnerId = _rnd.Next(111, 1000);
            var filter = EntityModellers.GetSentMessageReportRequestModel();
            var log = EntityModellers.GetSentLogsEntity();
            var logs = new List<LogEntity> { log };
            var sentReport = EntityModellers.GetSentMessageReportResponseModel();
            var sentReports = new List<SentMessageReportResponseModel> { sentReport };
            var expected = sentReports;

            mockContactRepository.Setup(x => x.GetChatUsersAsync(filter.SenderIds, partnerId)).ReturnsAsync(chatUserList);
            mockLogRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(logs);
            mapper.Setup(x => x.Map<IEnumerable<LogEntity>, IEnumerable<SentMessageReportResponseModel>>(logs)).Returns(expected);

            var logManager = new LogManager(mockContactRepository.Object, mockLogRepository.Object, mapper.Object);
            var actual = await logManager.AdminSentMessagesAsync(partnerId, filter);
            Assert.Equal(expected, actual, new LogicEqualityComparer<SentMessageReportResponseModel>());
            mockLogRepository.VerifyAll();
            mockContactRepository.VerifyAll();
        }

        [Fact]
        public async Task AdminAggregateUtilizationAsyncTest_WhenPartnerIdandFilter_ReturnsSentMessageReport()
        {
            var mockLogRepository = new Mock<ILogRepository>();
            var mockContactRepository = new Mock<IContactRepository>();

            var mockLogManager = new Mock<ILogManager>();
            var mapper = new Mock<IMapper>();

            var chatUser = EntityModellers.GetChatUserEntity();

            var employeeIds = new List<int>() { 1, 2, 3 };
            var chatUserList = new List<ChatUser> { chatUser };
            var partnerId = 2;
            var teamId = _rnd.Next(111, 1000);
            var filter = EntityModellers.GetAggregateUtilizationReportRequestModel();
            var log = new LogEntity
            {
                RecipientId = 1,
                RecepientName = "User1",
                ThreadId = Guid.NewGuid(),
                SenderId = 1,
                MessageId = Guid.NewGuid(),
                Message = "message",
                CreatedAt = Convert.ToDateTime("3/6/2019"),
                IsImportant = true,
                IsArchived = true,
                IsRead = true,
                ReadAt = DateTime.Now,
                ArchivedAt = DateTime.Now,
                TeamId = teamId
            };
            var logs1 = new List<LogEntity> { log };
            var aggregateReport = EntityModellers.GetAggregateUtilizationReportResponseModel();
            var aggregateReports = new List<AggregateUtilizationReportResponseModel> { aggregateReport };
            var expected = aggregateReports;

            mockContactRepository.Setup(x => x.GetChatUsersAsync(filter.EmployeeIds, partnerId)).ReturnsAsync(chatUserList);
            // mockLogRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(logs);
            mockLogRepository.Setup(x => x.GetByDatesAsync(filter, employeeIds)).ReturnsAsync(logs1);
            mapper.Setup(x => x.Map<IEnumerable<LogEntity>, IEnumerable<AggregateUtilizationReportResponseModel>>(logs1)).Returns(expected);

            var logManager = new LogManager(mockContactRepository.Object, mockLogRepository.Object, mapper.Object);
            var actual = await logManager.AdminAggregateUtilizationAsync(partnerId, filter);
            Assert.Equal(expected, actual, new LogicEqualityComparer<AggregateUtilizationReportResponseModel>());
            mockLogRepository.VerifyAll();
            mockContactRepository.VerifyAll();
        }

    }
}
