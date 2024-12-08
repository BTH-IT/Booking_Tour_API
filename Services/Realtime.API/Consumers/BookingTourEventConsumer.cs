using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class BookingTourEventConsumer : IConsumer<BookingTourEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;

        public BookingTourEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingTourEvent> context)
        {
            logger.Information("$START BookingTourEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("BookingTourEvent", context.Message);
            logger.Information("$END BookingTourEventConsumer - Consume ");
        }
    }
}
