using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Models
{
    [Table("RoleDetail")]
    public class RoleDetail
    {
        [Key]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Key]
        public string ActionName { get; set; }

        public bool Status { get; set; } = false;
    }
}
