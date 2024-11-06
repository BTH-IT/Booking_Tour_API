namespace Saga.Orchestrator.BookingTourOrderManagers
{
    public enum EBookingTourState
    {
        Initial,
        GetScheduleInfoInProcessing,
        ScheduleCheckInProcessing,
        InvoiceCreateInProcessing,
        UpdatingSchedule,
        Completed,
        Failed
    }
}
