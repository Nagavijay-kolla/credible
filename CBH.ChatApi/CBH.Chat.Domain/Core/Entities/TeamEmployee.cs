using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBH.Chat.Domain.Core.Entities
{
    [Table("TeamEmployee")]
    public class TeamEmployee
    {
        [Key]
        public int Team_Id { get; set; }
        public int Emp_Id { get; set; }
    }
}