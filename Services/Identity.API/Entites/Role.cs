using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Contracts.Domains;
using Contracts.Domains.Interfaces;

namespace Identity.API.Entites
{
    [Table("Roles")]
    public class Role : EntityBase<int> , IDateTracking
    {
        public string? RoleName { get; set; }
        public bool Status { get; set; }
        public ICollection<RoleDetail>? RoleDetails { get; set; }
        public ICollection<Account>? Accounts { get; set; } = new List<Account>();
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
