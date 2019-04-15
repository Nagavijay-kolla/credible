using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CBH.Chat.Domain.Core.Entities
{
    [Table("Profile")]
    public class Profile
    {
        [Key]
        public int Profile_Id { get; set; }
        public string Profile_Code { get; set; }
    }
}