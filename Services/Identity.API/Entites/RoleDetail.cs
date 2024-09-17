using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;
using Contracts.Domains;
namespace Identity.API.Entites
{
    [Table("RoleDetail")]
    public class RoleDetail : EntityBase<int>
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
