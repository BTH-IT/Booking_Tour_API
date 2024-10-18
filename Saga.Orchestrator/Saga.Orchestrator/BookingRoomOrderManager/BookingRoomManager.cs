using Booking.API.GrpcServer.Protos;
using Saga.Orchestrator.API.GrpcClient.Protos;
using Shared.DTOs;
using Stateless;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using ILogger = Serilog.ILogger;
namespace Saga.Orchestrator.BookingRoomOrderManager
{
    public class BookingRoomManager 
    {
        #region grpc
        private readonly IdentityGrpcService.IdentityGrpcServiceClient _identityGrpcServiceClient;
        private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
        private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;
        private readonly ILogger _logger;
        private readonly BookingGrpcService.BookingGrpcServiceClient _bookingGrpcServiceClient;
        #endregion

        private readonly IHttpContextAccessor _contextAccessor;
        private CreateBookingRoomOrderDto request;
        private GetRoomsByIdsResponse roomsInfo;
        private bool IsUpdateRooms  = false;
        private string? ErrorMessage;
        private readonly StateMachine<EBookingRoomState,EBookingRoomAction> _stateMachine;
        public BookingRoomManager(ILogger logger,
            IdentityGrpcService.IdentityGrpcServiceClient identityGrpcServiceClient,
            TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient,
            RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient,
            IHttpContextAccessor httpContextAccessor,
            BookingGrpcService.BookingGrpcServiceClient bookingGrpcServiceClient
            )
        {
           this._logger = logger;
           this._contextAccessor = httpContextAccessor;
           this._identityGrpcServiceClient = identityGrpcServiceClient;
           this._tourGrpcServiceClient = tourGrpcServiceClient;
           this._roomGrpcServiceClient = roomGrpcServiceClient;
           this._bookingGrpcServiceClient = bookingGrpcServiceClient;
            _stateMachine = new StateMachine<EBookingRoomState, EBookingRoomAction>(EBookingRoomState.Initial);

            // Lấy thông tin các phòng
            _stateMachine.Configure(EBookingRoomState.Initial)
                .Permit(EBookingRoomAction.GetRoomInfo, EBookingRoomState.GetRoomsInfoSuccess)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(GetRoomsInfoAsync);
            // Kiểm tra phòng trống
            _stateMachine.Configure(EBookingRoomState.GetRoomsInfoSuccess)
                .Permit(EBookingRoomAction.CheckRoomIsAvailable, EBookingRoomState.RoomsChecked)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(CheckRoomIsAvailableAsync);
            // Tạo hóa đơn
            _stateMachine.Configure(EBookingRoomState.RoomsChecked)
                .Permit(EBookingRoomAction.CreateBookingRoom, EBookingRoomState.InvoiceCreated)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(CreateBookingOrderAsync);
            // Cập nhật phòng 
            _stateMachine.Configure(EBookingRoomState.InvoiceCreated)
                .Permit(EBookingRoomAction.UpdateRoom, EBookingRoomState.Completed)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(async () => { });
            //Rollback khi thất bại
            _stateMachine.Configure(EBookingRoomState.Failed)
                .OnEntryAsync(async () => { });

        }
        private async Task GetRoomsInfoAsync()
        {
            try
            {
                var roomIds = this.request.BookingRoomDetails.Select(c => c.RoomId).ToList();
                if(roomIds.Count  == 0 )
                {
                    throw new Exception("Danh sách phòng trống");
                }
                var request = new GetRoomsByIdsRequest();
                request.Ids.AddRange(roomIds);
                roomsInfo = await _roomGrpcServiceClient.GetRoomsByIdsAsync(request);

                if(roomsInfo.Rooms.Count != roomIds.Count)
                {
                    throw new Exception("Có lỗi dữ liệu khi lấy dữ liệu phòng");
                }
                await _stateMachine.FireAsync(EBookingRoomAction.CheckRoomIsAvailable);
                
            }catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingRoomAction.Rollback);
            }
        }
        private async Task CheckRoomIsAvailableAsync()
        {
            
            try
            {
                foreach(var roomInfo in roomsInfo.Rooms)
                {
                    if(!roomInfo.IsAvailable)
                    {
                        throw new Exception($"{roomInfo.Name} hiện không khả dụng");
                    }    
                }
                await _stateMachine.FireAsync(EBookingRoomAction.CreateBookingRoom);
            }
            catch (Exception ex) 
            {
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingRoomAction.Rollback);
            }
        } 
        private async Task CreateBookingOrderAsync()
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
            try
            {
                
                await _stateMachine.FireAsync(EBookingRoomAction.UpdateRoom);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingRoomAction.Rollback);
            }
        }
        private async Task UpdateRoomAsync()
        {

        }
        public async Task<BookingRoomResponse> CreateBookingRoomOrder(CreateBookingRoomOrderDto input)
        {
            this.request = input;
            await _stateMachine.FireAsync(EBookingRoomAction.GetRoomInfo);
            if(_stateMachine.State == EBookingRoomState.Completed )
            {
                return null;
            }
            return null;
        }
    }
}
