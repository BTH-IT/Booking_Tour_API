namespace Saga.Orchestrator.BookingRoomOrderManagers
{
    public enum EBookingRoomState
    {
        Initial,
        GetRoomsInfoInProcessing,
        RoomsCheckInProcessing,
        InvoiceCreateInProcessing,
        RoomUpdating,
        Completed,
        Failed
    }
}
