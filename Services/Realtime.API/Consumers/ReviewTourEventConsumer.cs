using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class ReviewTourEventConsumer : IConsumer<ReviewTourEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;
        public ReviewTourEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<ReviewTourEvent> context)
        {
            logger.Information("$START ReviewTourEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("ReviewTourEvent",context.Message);
            logger.Information("$END ReviewTourEventConsumer - Consume ");
        }
    }
}
