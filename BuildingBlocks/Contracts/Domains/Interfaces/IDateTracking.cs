namespace Contracts.Domains.Interfaces
{
    public interface IDateTracking
    {
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; } 
    }
}
