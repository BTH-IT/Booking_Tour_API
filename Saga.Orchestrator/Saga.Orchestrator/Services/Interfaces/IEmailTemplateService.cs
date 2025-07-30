using Shared.DTOs;

namespace Saga.Orchestrator.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        Task SendBookingRoomConfirmationEmail(BookingRoomResponseDTO bookingRoom);
        Task SendBookingTourConfirmationEmail( BookingTourCustomResponseDTO bookingTour);
    }
}
