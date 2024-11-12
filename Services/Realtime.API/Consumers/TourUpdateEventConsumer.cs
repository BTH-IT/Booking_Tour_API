using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Realtime.API.Hubs;
using ILogger = Serilog.ILogger;
namespace Realtime.API.Consumers
{
    public class TourUpdateEventConsumer : IConsumer<ScheduleUpdateEvent>
    {
        private readonly IHubContext<NotiHub> hubContext;
        private readonly ILogger logger;
        public TourUpdateEventConsumer(IHubContext<NotiHub> hubContext,
            ILogger logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ScheduleUpdateEvent> context)
        {
            logger.Information("$START TourUpdateEventConsumer - Consume ");
            await hubContext.Clients.All.SendAsync("ScheduleUpdateEvent",new
            {
                ScheduleId = context.Message.ObjectId,
                AvailableSeats = context.Message.Data
            });
            logger.Information("$END TourUpdateEventConsumer - Consume ");
        }
    }
}
