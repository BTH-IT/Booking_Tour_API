using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class ReviewRoomEventConsumer : IConsumer<ReviewRoomEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;
        public ReviewRoomEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ReviewRoomEvent> context)
        {
            logger.Information("$START ReviewRoomEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("ReviewRoomEvent", context.Message);
            logger.Information("$END ReviewTourEventCReviewRoomEventConsumeronsumer - Consume ");
        }
    }
}
