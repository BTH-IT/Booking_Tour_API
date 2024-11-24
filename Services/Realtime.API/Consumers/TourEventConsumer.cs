using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;

namespace Realtime.API.Consumers
{
    public class TourEventConsumer : IConsumer<TourEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;

        public TourEventConsumer(IHubContext<NotiHub> hubContext, ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<TourEvent> context)
        {
            logger.Information("$START TourEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("TourEvent", context.Message);
            logger.Information("$END TourEventConsumer - Consume ");
        }
    }
}
