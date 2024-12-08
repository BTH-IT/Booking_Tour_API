using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class HotelEventConsumer : IConsumer<HotelEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;

        public HotelEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<HotelEvent> context)
        {
            logger.Information("$START HotelEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("HotelEvent", context.Message);
            logger.Information("$END HotelEventConsumer - Consume ");
        }
    }
}
