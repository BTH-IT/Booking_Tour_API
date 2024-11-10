

namespace EventBus.Masstransit
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ushort Port { get; set; }    
    }
}
