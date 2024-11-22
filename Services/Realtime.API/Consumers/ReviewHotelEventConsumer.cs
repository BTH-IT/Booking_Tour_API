using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class ReviewHotelEventConsumer : IConsumer<ReviewHotelEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;
        public ReviewHotelEventConsumer(IHubContext<NotiHub> hubContext,ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;   
        }
        public async Task Consume(ConsumeContext<ReviewHotelEvent> context)
        {
            logger.Information("$START ReviewHotelEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("ReviewHotelEvent", context.Message);
            logger.Information("$END ReviewHotelEventConsumer - Consume ");
        }
    }
}
