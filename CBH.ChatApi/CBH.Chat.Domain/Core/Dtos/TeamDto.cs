using System.Collections.Generic;

namespace CBH.Chat.Domain.Core.Dtos
{
    public class TeamDto
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IList<TeamMemberDto> TeamMembers { get; set; }
    }
}