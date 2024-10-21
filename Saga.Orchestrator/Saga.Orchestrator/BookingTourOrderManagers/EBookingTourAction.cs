namespace Saga.Orchestrator.BookingTourOrderManagers
{
    public enum EBookingTourAction
    {
        GetScheduleInfo,
        GetRoomInfo,
        CheckScheduleIsAvailable,
        CheckRoomIsAvailable,
        CreateBookingTour,
        UpdateScheduleSeat,
        Finish,
        Rollback
    }
}
