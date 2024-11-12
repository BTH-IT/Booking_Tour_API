
namespace EventBus.IntergrationEvents.Interfaces
{
    public interface IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }
        public Guid Id { get; set; }
        public int ObjectId { get; set; }

    }
}
