using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? RoleName { get; set; }

        public bool Status { get; set; }

        public ICollection<RoleDetail> RoleDetails { get; set; }
    }
}
