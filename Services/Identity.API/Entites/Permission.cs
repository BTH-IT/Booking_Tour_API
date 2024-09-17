using Contracts.Domains;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Entites
{
    [Table("Permission")]
    public class Permission : EntityBase<int>
    {
        public string Name { get; set; }
        public bool Status { get; set; } = false;
        public ICollection<RoleDetail> RoleDetails { get; set; }
    }
}
