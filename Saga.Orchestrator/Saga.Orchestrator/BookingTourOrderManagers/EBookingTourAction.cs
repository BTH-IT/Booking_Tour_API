namespace Saga.Orchestrator.BookingTourOrderManagers
{
    public enum EBookingTourAction
    {
        GetScheduleInfo,
        CheckScheduleIsAvailable,
        CreateBookingTour,
        UpdateScheduleSeat,
        Finish,
        Rollback
    }
}
