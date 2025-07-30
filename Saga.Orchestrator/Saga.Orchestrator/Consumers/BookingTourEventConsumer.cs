using EventBus.IntergrationEvents.Events;
using MassTransit;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Consumers
{
    public class BookingTourEventConsumer : IConsumer<BookingTourEvent>
    {
        private readonly IEmailTemplateService _emailService;
        private readonly ILogger _logger;

        public BookingTourEventConsumer(IEmailTemplateService emailService, ILogger logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingTourEvent> context)
        {
            try
            {
                _logger.Information($"Received BookingTourEvent with ID: {context.Message.Id}, Type: {context.Message.Type}");
                
                var bookingTour = context.Message.Data;
                
                // Only send confirmation for CREATE events
                if (context.Message.Type == "CREATE")
                {
                    await _emailService.SendBookingTourConfirmationEmail(bookingTour);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error processing BookingTourEvent: {ex.Message}");
            }
        }
    }
}