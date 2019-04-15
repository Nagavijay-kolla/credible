using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserThreadController : ControllerBase
    {
        private readonly IThreadManager _threadManager;

        public UserThreadController(IThreadManager threadManager)
        {
            _threadManager = threadManager;
        }

        /// <summary>
        /// Get all Threads of a User
        /// </summary>
        [HttpGet("{userId}/threads")]
        public async Task<ActionResult<ThreadWithContactsResponseModel>> GetAllThreadsByUserIdAsync([FromRoute]int userId, [FromQuery(Name = "limit")] int limit = -1, [FromQuery(Name = "type")]FetchThreadType fetchType = FetchThreadType.All)
        {
            return Ok(await _threadManager.GetThreadsByUserId(userId, limit, fetchType));
        }
    }
}