using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPatch("{chatUserId}/status/{status}")]
        public async Task<ActionResult<UserResponseModel>> UpdateUserStatusAsync(int chatUserId, [FromRoute]UserStatus status)
        {
            return Ok(await _userManager.UpdateUserStatusAsync(chatUserId, status));
        }
    }
}