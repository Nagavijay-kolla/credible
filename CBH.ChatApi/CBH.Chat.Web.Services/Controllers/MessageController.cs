using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IThreadManager _threadManager;

        public MessageController(IThreadManager threadManager)
        {
            _threadManager = threadManager;
        }

        /// <summary>
        /// Send new message on a thread
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<ThreadWithMessagesResponseModel>> AddNewMessage([FromBody]NewMessageRequestModel newMessage)
        {            
            return Ok(await _threadManager.AddNewMessageAsync(newMessage));
        }

        /// <summary>
        /// Send new broadcast message
        /// </summary>
        [HttpPost("broadcast")]
        public async Task<ActionResult<ThreadWithMessagesResponseModel>> AddNewBroadcastMessage([FromBody]NewMessageRequestModel newMessage)
        {
            return Ok(await _threadManager.AddNewMessageAsync(newMessage));
        }

        /// <summary>
        /// Set read status of messages
        /// </summary>
        [HttpPut("read")]
        public async Task<ActionResult> SetMessageReadStatus([FromQuery(Name = "user_id")]int userId, [FromBody]ReadRequestModel readRequestModel)
        {
            await _threadManager.SetReadStatusAsync(userId, readRequestModel.MessageIds);
            return Ok();
        }
    }
}