using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserContactController : ControllerBase
    {
        private readonly IContactManager _contactManager;

        public UserContactController(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }

        /// <summary>
        /// Get all Contacts (with details) of a User
        /// </summary>
        [HttpGet("{chatUserId}/contacts")]
        public async Task<ActionResult<ICollection<ContactUsersDto>>> GetContactsByUserIdAsync([FromRoute]int chatUserId)
        {
            return Ok(await _contactManager.GetContactsAsync(chatUserId));
        }

        [HttpGet("{chatUserId}/searchContacts")]
        public async Task<ActionResult<ICollection<ContactUsersDto>>> SearchContactsAsync([FromRoute]int chatUserId, [FromQuery]string searchString)
        {
            return Ok(await _contactManager.SearchContactsAsync(chatUserId, searchString));
        }

        [HttpGet("{chatUserId}/groupContacts/{groupId}")]
        public async Task<ActionResult<ICollection<ContactUsersDto>>> GetGroupContactsSearchAsync([FromRoute]int chatUserId, [FromRoute]Guid groupId)
        {
            return Ok(await _contactManager.GetGroupContactsSearch(chatUserId, groupId));
        }

        /// <summary>
        /// Add new Contact to a User
        /// </summary>
        [HttpPut("{chatUserId}/contact")]
        public async Task<IActionResult> AddNewContactToUser([FromRoute]int chatUserId, [FromQuery]int contactUserId)
        {
            return Ok(await _contactManager.AddContactAsync(chatUserId, contactUserId));
        }

        /// <summary>
        /// Remove Contact of a User
        /// </summary>
        [HttpDelete("{chatUserId}/contact")]
        public async Task<IActionResult> RemoveContactOfUser([FromRoute]int chatUserId, [FromQuery]int contactUserId)
        {
            return Ok(await _contactManager.DeleteContactAsync(chatUserId, contactUserId));
        }

        [HttpGet]
        [Route("{chatUserId}/myContacts")]
        public async Task<IActionResult> GetMyContactsAsync([FromRoute]int chatUserId)
        {
            return Ok(await _contactManager.GetMyContactsAsync(chatUserId));
        }
    }
}