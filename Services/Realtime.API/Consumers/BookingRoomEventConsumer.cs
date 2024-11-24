using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class BookingRoomEventConsumer : IConsumer<BookingRoomEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;

        public BookingRoomEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingRoomEvent> context)
        {
            logger.Information("$START BookingRoomEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("BookingRoomEvent", context.Message);
            logger.Information("$END BookingRoomEventConsumer - Consume ");
        }
    }
}
