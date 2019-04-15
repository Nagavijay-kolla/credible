using System;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("thread")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly IThreadManager _threadManager;

        public ThreadController(IThreadManager threadManager)
        {
            _threadManager = threadManager;
        }

        /// <summary>
        /// Gets the Thread by Id
        /// </summary>
        [HttpGet("{threadId}")]
        public async Task<ActionResult<ThreadWithMessagesResponseModel>> GetThreadByIdAsync([FromRoute]Guid threadId)
        {
            return Ok(await _threadManager.GetThreadById(threadId));
        }

        /// <summary>
        /// Create (or Get) Thread with all messages
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<ThreadWithMessagesResponseModel>> CreateOrGetThreadAsync([FromQuery(Name = "user_id")]int userId, [FromQuery(Name = "to_id")]string participantId, [FromQuery(Name = "type")]ThreadType threadyType = ThreadType.User)
        {
            return Ok(await _threadManager.SearchOrCreateThreadAsync(userId, participantId, threadyType));
        }

        /// <summary>
        /// Toggle Thread archive status
        /// </summary>
        [HttpPatch("{threadId}/archive")]
        public async Task<ActionResult> ToggleArchive([FromRoute]Guid threadId, [FromQuery(Name = "user_id")]int userId, [FromQuery(Name = "status")]bool status = false)
        {
            await _threadManager.ArchiveThreadyByIdAsync(userId, threadId, status);
            return Ok();
        }

        /// <summary>
        /// Get Broadcast thread and messages
        /// </summary>
        [HttpPut("broadcast")]
        public async Task<ActionResult<ThreadWithMessagesResponseModel>> GetBroadcastThreadAsync()
        {
            return Ok(await _threadManager.SearchOrCreateBroadcastThreadAsync());
        }
    }
}