﻿using Booking.API.GrpcServer.Protos;
using Google.Protobuf.WellKnownTypes;
using MassTransit;
using Saga.Orchestrator.API.GrpcClient.Protos;
using Shared.Constants;
using Shared.DTOs;
using Shared.Helper;
using Stateless;
using System.Security.Claims;
using ILogger = Serilog.ILogger;
namespace Saga.Orchestrator.BookingRoomOrderManagers
{
    public class BookingRoomManager 
    {
        #region grpc
        private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
        private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;
        private readonly ILogger _logger;
        private readonly BookingGrpcService.BookingGrpcServiceClient _bookingGrpcServiceClient;
        #endregion

        private readonly IHttpContextAccessor _contextAccessor;
        private CreateBookingRoomOrderDto requestDto;
        private GetRoomsByIdsResponse roomsInfo;
        private string? ErrorMessage;
        private int BookingRoomId = -1;
        private readonly StateMachine<EBookingRoomState,EBookingRoomAction> _stateMachine;
        public BookingRoomManager(ILogger logger,
            TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient,
            RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient,
            IHttpContextAccessor httpContextAccessor,
            BookingGrpcService.BookingGrpcServiceClient bookingGrpcServiceClient
            )
        {
           this._logger = logger;
           this._contextAccessor = httpContextAccessor;
           this._tourGrpcServiceClient = tourGrpcServiceClient;
           this._roomGrpcServiceClient = roomGrpcServiceClient;
           this._bookingGrpcServiceClient = bookingGrpcServiceClient;
            _stateMachine = new StateMachine<EBookingRoomState, EBookingRoomAction>(EBookingRoomState.Initial);

            // Trạng thái ban đầu 
            _stateMachine.Configure(EBookingRoomState.Initial)
                .Permit(EBookingRoomAction.GetRoomInfo, EBookingRoomState.GetRoomsInfoInProcessing)
                .OnEntry(()=> logger.Information("Entered Initial State"));
            // Lấy thông tin phòng
            _stateMachine.Configure(EBookingRoomState.GetRoomsInfoInProcessing)
                .Permit(EBookingRoomAction.CheckRoomIsAvailable, EBookingRoomState.RoomsCheckInProcessing)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(GetRoomsInfoAsync);

            // Kiểm tra phòng trống
            _stateMachine.Configure(EBookingRoomState.RoomsCheckInProcessing)
                .Permit(EBookingRoomAction.CreateBookingRoom, EBookingRoomState.InvoiceCreateInProcessing)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(CheckRoomIsAvailableAsync);
            // Tạo hóa đơn
            _stateMachine.Configure(EBookingRoomState.InvoiceCreateInProcessing)
                .Permit(EBookingRoomAction.Finish, EBookingRoomState.Completed)
                .Permit(EBookingRoomAction.Rollback, EBookingRoomState.Failed)
                .OnEntryAsync(CreateBookingOrderAsync);
            //Rollback khi thất bại
            _stateMachine.Configure(EBookingRoomState.Failed)
                .OnEntryAsync(RollbackAsync);
        }
        private async Task GetRoomsInfoAsync()
        {
            try
            {
                _logger.Information("Begin : GetRoomsInfoAsync - BookingRoomManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                var roomIds = this.requestDto.BookingRoomDetails.Select(c => c.RoomId).ToList();
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
                _logger.Information("End : GetRoomsInfoAsync - BookingRoomManager");

                await _stateMachine.FireAsync(EBookingRoomAction.CheckRoomIsAvailable);
                
            }catch(Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : GetRoomsInfoAsync - BookingRoomManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingRoomAction.Rollback);
            }
        }
        private async Task CheckRoomIsAvailableAsync()
        {
            
            try
            {
                _logger.Information("Begin : CheckRoomIsAvailableAsync - BookingRoomManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                
                var roomIds = roomsInfo.Rooms.Select(c => c.Id);
                var request = new CheckRoomsIsBookedRequest();
                request.RoomIds.AddRange(roomIds);
                request.CheckIn = Timestamp.FromDateTime(requestDto.CheckIn!.Value.ToUniversalTime()) ;
                request.CheckOut = Timestamp.FromDateTime(requestDto.CheckOut!.Value.ToUniversalTime());
                var response = await _bookingGrpcServiceClient.CheckRoomsIsBookedAsync(request);
                
                
                if(response.Result == false)
                {
                    throw new Exception(response.Message);
                }
                foreach(var item in requestDto.BookingRoomDetails)
                {
                    var roomInfo = roomsInfo.Rooms.FirstOrDefault(c=>c.Id == item.RoomId);
                    if(roomInfo!.MaxGuests < (item.Adults + item.Children))
                    {
                        throw new Exception($"Phòng với id :{roomInfo.Id} cho phép tối đa {roomInfo.MaxGuests} người");
                    }
                }    

                await _stateMachine.FireAsync(EBookingRoomAction.CreateBookingRoom);
            }
            catch (Exception ex) 
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : CheckRoomIsAvailableAsync - BookingRoomManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingRoomAction.Rollback);
            }
        } 
        private async Task CreateBookingOrderAsync()
        {
            var userId = int.Parse(_contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                _logger.Information("Begin : CheckRoomIsAvailableAsync - BookingRoomManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                var request = new CreateBookingRoomRequest()
                {
                    UserId = userId,
                    CheckIn = Timestamp.FromDateTime(requestDto.CheckIn!.Value.ToUniversalTime()),
                    CheckOut = Timestamp.FromDateTime(requestDto.CheckOut!.Value.ToUniversalTime()),
                    Status = Constants.OrderStatus.Pending
                };
                foreach(var item in requestDto.BookingRoomDetails)
                {
                    request.BookingRoomDetails.Add(new BookingRoomDetailRequest
                    {
                        RoomId = item.RoomId,
                        Adult= item.Adults,
                        Children = item.Children,
                        Price = item.Price
                    });
                }
                var response = await _bookingGrpcServiceClient.CreateBookingRoomAsync(request);
                this.BookingRoomId = response.BookingRoomId;    
                await _stateMachine.FireAsync(EBookingRoomAction.Finish);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : CreateBookingOrderAsync - BookingRoomManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingRoomAction.Rollback);
            }
        }
        private async Task RollbackAsync()
        {
            try
            {
                _logger.Information("Begin : RollbackAsync - BookingRoomManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                if (this.BookingRoomId > 0)
                {
                    await _bookingGrpcServiceClient.DeleteBookingRoomAsync(new DeleteBookingRoomRequest()
                    {
                        BookingRoomId = this.BookingRoomId  
                    });
                }
            }
            catch (Exception ex) 
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : RollbackAsync - BookingRoomManager");
            }
        }
        public async Task<ApiResponse<BookingRoomResponse>> CreateBookingRoomOrder(CreateBookingRoomOrderDto input)
        {
            this.requestDto = input;
            await _stateMachine.FireAsync(EBookingRoomAction.GetRoomInfo);
            if(_stateMachine.State == EBookingRoomState.Completed )
            {
                return new ApiResponse<BookingRoomResponse>(200,new BookingRoomResponse
                {
                    BookingRoomId = this.BookingRoomId
                },"Đặt phòng thành công");
            }
            return new ApiResponse<BookingRoomResponse>(400,null, this.ErrorMessage); ;
        }
    }
}
