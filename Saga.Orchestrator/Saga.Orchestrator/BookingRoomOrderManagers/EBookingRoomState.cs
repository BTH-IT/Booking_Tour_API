namespace Saga.Orchestrator.BookingRoomOrderManagers
{
    public enum EBookingRoomState
    {
        Initial,
        GetRoomsInfoInProcessing,
        RoomsCheckInProcessing,
        InvoiceCreateInProcessing,
        Completed,
        Failed
    }
}
