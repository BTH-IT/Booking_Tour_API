using EventBus.IntergrationEvents.Events;
using MassTransit;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Consumers
{
    public class BookingRoomEventConsumer : IConsumer<BookingRoomEvent>
    {
        private readonly IEmailTemplateService _emailService;
        private readonly ILogger _logger;

        public BookingRoomEventConsumer(IEmailTemplateService emailService, ILogger logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingRoomEvent> context)
        {
            try
            {
                _logger.Information($"Received BookingRoomEvent with ID: {context.Message.Id}, Type: {context.Message.Type}");
                
                var bookingRoom = context.Message.Data;
                
                // Only send confirmation for CREATE events
                if (context.Message.Type == "CREATE")
                {
                    await _emailService.SendBookingRoomConfirmationEmail(bookingRoom);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error processing BookingRoomEvent: {ex.Message}");
            }
        }
    }
}