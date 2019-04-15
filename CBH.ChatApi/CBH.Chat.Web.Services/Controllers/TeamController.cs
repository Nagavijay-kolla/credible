using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Business;
using Microsoft.AspNetCore.Mvc;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamManager _teamManager;

        public TeamController(ITeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<IActionResult> GetTeams(int userId)
        {
            return Ok(await _teamManager.GetTeams(userId));
        }

        [HttpGet]
        [Route("members/{teamId}")]
        public async Task<ActionResult<IEnumerable<TeamMemberDto>>> GetTeamMembers(int teamId)
        {
            return Ok(await _teamManager.GetTeamMembersAsync(teamId));
        }
    }
}