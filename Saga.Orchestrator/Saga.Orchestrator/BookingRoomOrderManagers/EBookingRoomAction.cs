namespace Saga.Orchestrator.BookingRoomOrderManagers
{
    public enum EBookingRoomAction
    {
        GetRoomInfo,
        CheckRoomIsAvailable,
        CreateBookingRoom,
        UpdateRoom,
        Rollback,
        Finish
    }
}
