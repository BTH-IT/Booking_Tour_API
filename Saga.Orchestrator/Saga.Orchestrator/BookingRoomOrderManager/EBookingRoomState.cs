namespace Saga.Orchestrator.BookingRoomOrderManager
{
    public enum EBookingRoomState
    {
        Initial,
        GetRoomsInfoSuccess,
        RoomsChecked,
        InvoiceCreated,
        RoomUpdated,
        Completed,
        Failed
    }
}
