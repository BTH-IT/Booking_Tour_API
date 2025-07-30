

namespace EventBus.Masstransit
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ushort Port { get; set; }
        public string VHost { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;

        public string ReceiveBookingEventQueue = string.Empty;

    }
}
