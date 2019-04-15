using System.Threading.Tasks;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("userConfiguration")]
    public class UserConfigurationController : Controller
    {
        private readonly IUserConfigurationManager _userConfigurationManager;

        public UserConfigurationController(IUserConfigurationManager userConfigurationManager)
        {
            _userConfigurationManager = userConfigurationManager;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<UserConfigurationDto>> GetUserConfiguration(int userId)
        {
            return Ok(await _userConfigurationManager.GetUserChatConfigurationAsync(userId));
        }
    }
}