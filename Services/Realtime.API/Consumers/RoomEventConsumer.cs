using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class RoomEventConsumer : IConsumer<RoomEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;

        public RoomEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<RoomEvent> context)
        {
            logger.Information("$START RoomEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("RoomEvent", context.Message);
            logger.Information("$END RoomEventConsumer - Consume ");
        }
    }
}
