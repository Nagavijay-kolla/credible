using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CBH.Chat.Domain.Core.Entities
{
    [Table("ProfileSecurity")]
    public class ProfileSecurity 
    {
        [Key]
        public int Security_Id { get; set; }
        public int Profile_Id { get; set; }
    }
}