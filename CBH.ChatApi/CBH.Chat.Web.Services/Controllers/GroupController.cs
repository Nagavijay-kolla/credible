using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupManager _groupManager;

        public GroupController(IGroupManager groupManager)
        {
            _groupManager = groupManager;
        }

        /// <summary>
        /// Get Group details
        /// </summary>
        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupWithUsersResponseModel>> GetGroupAsync([FromRoute]Guid groupId)
        {
            return Ok(await _groupManager.FetchByIdAsync(groupId));
        }

        /// <summary>
        /// Create new Group
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<GroupResponseModel>> CreateGroupAsync([FromBody]NewGroupRequestModel newGroup)
        {
            return Ok(await _groupManager.CreateNewGroupAsync(newGroup));
        }

        /// <summary>
        /// Delete a Group
        /// </summary>
        [HttpDelete("{groupId}")]
        public async Task<ActionResult<GroupResponseModel>> DeleteGroupAsync([FromRoute]Guid groupId)
        {
            return Ok(await _groupManager.DeleteGroupByIdAsync(groupId));
        }

        /// <summary>
        /// Add User to a Group
        /// </summary>
        [HttpPut("{groupId}/addcontact")]
        public async Task<ActionResult<GroupResponseModel>> AddUsersToGroupAsync([FromRoute]Guid groupId, [FromQuery(Name ="user_id")]IList<int> userIds)
        {
            return Ok(await _groupManager.AddUsersToGroupAsync(groupId, userIds));
        }

        /// <summary>
        /// Remove User to a Group
        /// </summary>
        [HttpDelete("{groupId}/deletecontact")]
        public async Task<ActionResult<GroupResponseModel>> DeleteUserFromGroupAsync([FromRoute]Guid groupId, [FromQuery(Name ="user_id")]IList<int> userIds)
        {
            return Ok(await _groupManager.DeleteUsersFromGroupAsync(groupId, userIds));
        }

        [HttpGet("{groupId}/getMembers")]
        public async Task<ActionResult<UserContactResponseModel>> GetGroupMembersAsync([FromRoute]Guid groupId)
        {
            return Ok(await _groupManager.FetchGroupUserByIdAsync(groupId));
        }
    }
}