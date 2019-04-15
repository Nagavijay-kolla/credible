using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IGroupManager _groupManager;

        public UserGroupController(IGroupManager groupManager)
        {
            _groupManager = groupManager;
        }

        /// <summary>
        /// Get all Groups of a User
        /// </summary>
        [HttpGet("{userId}/groups")]
        public async Task<ActionResult<IEnumerable<GroupResponseModel>>> GetAllUserGroupsAsync([FromRoute]int userId)
        {
            return Ok(await _groupManager.FetchByUserIdAsync(userId));
        }
    }
}