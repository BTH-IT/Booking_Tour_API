using Booking.API.GrpcServer.Protos;
using Google.Protobuf.WellKnownTypes;
using ILogger = Serilog.ILogger;
using Saga.Orchestrator.API.GrpcClient.Protos;
using Shared.DTOs;
using Saga.Orchestrator.BookingRoomOrderManagers;
using Stateless;

namespace Saga.Orchestrator.BookingTourOrderManagers
{
    public class BookingTourManager
    {
        #region grpc
        private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
        private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;
        private readonly ILogger _logger;
        private readonly BookingGrpcService.BookingGrpcServiceClient _bookingGrpcServiceClient;
        #endregion
        private readonly IHttpContextAccessor _contextAccessor;
        private CreateBookingTourOrderDto? requestDto;
        private string? Message;
        private int BookingTourId = -1;
        private readonly StateMachine<EBookingTourState, EBookingTourAction> _stateMachine;

        public BookingTourManager(TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient, 
            RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient, 
            ILogger logger, 
            BookingGrpcService.BookingGrpcServiceClient bookingGrpcServiceClient, 
            IHttpContextAccessor contextAccessor)
        {
            _tourGrpcServiceClient = tourGrpcServiceClient;
            _roomGrpcServiceClient = roomGrpcServiceClient;
            _logger = logger;
            _bookingGrpcServiceClient = bookingGrpcServiceClient;
            _contextAccessor = contextAccessor;

            _stateMachine = new StateMachine<EBookingTourState, EBookingTourAction>(EBookingTourState.Initial);

            // Trạng thái ban đầu  -> lấy thông tin lịch trình
            _stateMachine.Configure(EBookingTourState.Initial)
                .Permit(EBookingTourAction.GetScheduleInfo, EBookingTourState.GetScheduleInfoInProcessing)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(async () => { });
            // Lấy thông tin phòng 
            _stateMachine.Configure(EBookingTourState.GetScheduleInfoInProcessing)
                .Permit(EBookingTourAction.GetRoomInfo, EBookingTourState.GetRoomsInfoInProcessing)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(async () => { });
            //Kiểm tra chỗ trống ở lịch trình
            _stateMachine.Configure(EBookingTourState.GetRoomsInfoInProcessing)
                .Permit(EBookingTourAction.CheckScheduleIsAvailable, EBookingTourState.ScheduleCheckInProcessing)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(async () => { });
            // Kiểm tra chỗ trông ở phòng
            _stateMachine.Configure(EBookingTourState.ScheduleCheckInProcessing)
                .Permit(EBookingTourAction.CreateBookingTour, EBookingTourState.InvoiceCreateInProcessing)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(async () => { });

            //Tạo hóa đơn
            _stateMachine.Configure(EBookingTourState.InvoiceCreateInProcessing)
                .Permit(EBookingTourAction.UpdateScheduleSeat, EBookingTourState.UpdatingSchedule)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(async () => { });
            
            //Cập nhật lịch trình
            _stateMachine.Configure(EBookingTourState.UpdatingSchedule)
                .Permit(EBookingTourAction.Finish, EBookingTourState.Completed)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(async () => { });
            // Rollback
            _stateMachine.Configure(EBookingTourState.Failed)
                .OnEntryAsync(async () => { });
        }
    }
}
