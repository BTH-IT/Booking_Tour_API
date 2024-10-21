namespace Saga.Orchestrator.BookingTourOrderManagers
{
    public enum EBookingTourState
    {
        Initial,
        GetRoomsInfoInProcessing,
        GetScheduleInfoInProcessing,
        RoomCheckInProcessing,
        ScheduleCheckInProcessing,
        InvoiceCreateInProcessing,
        UpdatingSchedule,
        UpdatingRoom,
        Completed,
        Failed
    }
}
