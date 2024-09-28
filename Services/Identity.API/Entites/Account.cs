using Contracts.Domains;
using Contracts.Domains.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Entites
{
    [Table("Accounts")]
    public class Account : EntityBase<int> , IDateTracking
    {
        public string Email { get;set; }
        public string Password { get;set; } 
        public string? RefreshToken { get;set; }
        public User? User { get;set; }
        public int RoleId { get;set; }  
        public Role? Role { get;set; }
        public DateTime CreatedAt { get; set ; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
