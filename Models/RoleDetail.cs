using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Models
{
    public enum ActionType
    {
        Create,
        Update,
        Delete,
        Read
    }

    [Table("RoleDetail")]
    public class RoleDetail
    {
        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int PermissionId { get; set; }

        [Key]
        [Column(Order = 3)]
        [EnumDataType(typeof(ActionType))]
        [DataType("integer")]
        public ActionType ActionName { get; set; }

        public bool Status { get; set; } = false;

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
