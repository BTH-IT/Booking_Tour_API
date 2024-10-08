using Contracts.Domains;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Entites
{
    [Table("Permissions")]
    public class Permission : EntityBase<int>
    {
        public string Name { get; set; }
        public bool Status { get; set; } = false;
        public ICollection<RoleDetail>? RoleDetails { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
