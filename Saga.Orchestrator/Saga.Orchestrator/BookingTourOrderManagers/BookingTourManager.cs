using Booking.API.GrpcServer.Protos;
using ILogger = Serilog.ILogger;
using Saga.Orchestrator.API.GrpcClient.Protos;
using Shared.DTOs;
using Stateless;
using Shared.Helper;
using System.Security.Claims;


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
        private string ErrorMessage = "";
        private int BookingTourId = -1;
        private ScheduleResponse? scheduleResponse;
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
                .OnEntry(() => logger.Information("Entered Initial State"));
            // Kiểm tra chỗ trống của lịch trình
            _stateMachine.Configure(EBookingTourState.GetScheduleInfoInProcessing)
                .Permit(EBookingTourAction.CheckScheduleIsAvailable, EBookingTourState.ScheduleCheckInProcessing)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(GetScheduleInfoAsync);
            //Kiểm tra chỗ trống ở lịch trình
            _stateMachine.Configure(EBookingTourState.ScheduleCheckInProcessing)
                .Permit(EBookingTourAction.CreateBookingTour, EBookingTourState.InvoiceCreateInProcessing)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(CheckScheduleIsAvailableAsync);
            //Cập nhật lịch trình
            _stateMachine.Configure(EBookingTourState.InvoiceCreateInProcessing)
                .Permit(EBookingTourAction.UpdateScheduleSeat, EBookingTourState.UpdatingSchedule)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(CreateBookingTourAsync);
            // Cập nhật thành công thì kết thúc
            _stateMachine.Configure(EBookingTourState.UpdatingSchedule)
                .Permit(EBookingTourAction.Finish, EBookingTourState.Completed)
                .Permit(EBookingTourAction.Rollback, EBookingTourState.Failed)
                .OnEntryAsync(UpdateScheduleSeatAsync);
            // Rollback
            _stateMachine.Configure(EBookingTourState.Failed)
                .OnEntryAsync(RollBackAsync);
        }
        private async Task GetScheduleInfoAsync()
        {
            try
            {
                _logger.Information("Begin : GetScheduleInfoAsync - BookingTourManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
       
                var request = new GetScheduleByIdRequest()
                {
                    Id =  requestDto!.ScheduleId
                };
                var response = await _tourGrpcServiceClient.GetScheduleByIdAsync(request);

                this.scheduleResponse = response.Schedule;
                if (this.scheduleResponse == null)
                {
                    throw new Exception("Có lỗi dữ liệu khi lấy dữ liệu lịch trình");
                }
                _logger.Information("End : GetRoomsInfoAsync - BookingTourManager");

                await _stateMachine.FireAsync(EBookingTourAction.CheckScheduleIsAvailable);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : GetScheduleInfoAsync - BookingTourManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingTourAction.Rollback);
            }
        }
        private async Task CheckScheduleIsAvailableAsync()
        {
            try
            {
                _logger.Information("Begin : CheckScheduleIsAvailableAsync - BookingTourManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                if(scheduleResponse!.AvailableSeats - requestDto!.Seats < 0)
                {
                    throw new Exception($"Tour không đủ chỗ cho {requestDto.Seats} người");
                }    
                _logger.Information("End : CheckScheduleIsAvailableAsync - BookingTourManager");

                await _stateMachine.FireAsync(EBookingTourAction.CreateBookingTour);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : CheckScheduleIsAvailableAsync - BookingTourManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingTourAction.Rollback);
            }
        }
        private async Task CreateBookingTourAsync()
        {
            try
            {
                var userId = int.Parse(_contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                #region tao_request
                var request = new CreateBookingTourRequest()
                {
                    UserId = userId,
                    ScheduleId = requestDto!.ScheduleId,
                    Seats = requestDto!.Seats,
                    IsTip = requestDto!.IsTip,  
                    IsEntranceTicket = requestDto!.IsEntranceTicket,
                    Status = requestDto.Status,
                    PriceTotal = double.Parse(requestDto.PriceTotal.ToString()),
                };
                foreach(var item in requestDto.Travellers)
                {
                    request.TravellerDetail.Add(new TravellerDetail
                    {
                        Gender = item.Gender,
                        FullName = item.Fullname,
                        Age = item.Age,
                        Phone = item.Phone,
                    });
                }
                #endregion

                var response = await _bookingGrpcServiceClient.CreateBookingTourAsync(request);
                this.BookingTourId = response.BookingTourId;
                await _stateMachine.FireAsync(EBookingTourAction.UpdateScheduleSeat);

                _logger.Information("End : CreateBookingTourAsync - BookingTourManager");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : CreateBookingTourAsync - BookingTourManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingTourAction.Rollback);
            }
        }
        private async Task UpdateScheduleSeatAsync()
        {
            try
            {
                _logger.Information("Begin : UpdateScheduleSeatAsync - BookingTourManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                var response = await _tourGrpcServiceClient.UpdateScheduleAvailableSeatAsync(new UpdateScheduleAvailableSeatRequest
                {
                    ScheduleId = this.scheduleResponse!.Id,
                    Count = this.requestDto!.Seats
                });
                if(response.Result == false)
                {
                    throw new Exception(response.Message);
                }    
                _logger.Information("End : UpdateScheduleSeatAsync - BookingTourManager");
                await _stateMachine.FireAsync(EBookingTourAction.Finish);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : UpdateScheduleSeatAsync - BookingTourManager");
                ErrorMessage = ex.Message;
                await _stateMachine.FireAsync(EBookingTourAction.Rollback);
            }
        }
        private async Task RollBackAsync()
        {
            try
            {
                _logger.Information("Begin : RollbackAsync - BookingTourManager");
                _logger.Information($"State Machine : {_stateMachine.State} ");
                if (this.BookingTourId > 0)
                {
                    await _bookingGrpcServiceClient.DeleteBookingTourAsync(new DeleteBookingTourRequest
                    {
                        BookingTourId  = this.BookingTourId,
                    });
                    await _tourGrpcServiceClient.UpdateScheduleAvailableSeatAsync(new UpdateScheduleAvailableSeatRequest
                    {
                        ScheduleId = this.scheduleResponse!.Id,
                        Count = this.requestDto!.Seats * (-1)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _logger.Error("End : RollbackAsync - BookingTourManager");
            }
   
        }
        public async Task<ApiResponse<BookingTourResponse>> CreateBookingTourOrder(CreateBookingTourOrderDto input)
        {
            this.requestDto = input;
            await _stateMachine.FireAsync(EBookingTourAction.GetScheduleInfo);
            if (_stateMachine.State == EBookingTourState.Completed)
            {
                return new ApiResponse<BookingTourResponse>(200, new BookingTourResponse
                {
                    BookingTourId = this.BookingTourId
                }, "Đặt tour thành công");
            }
            return new ApiResponse<BookingTourResponse>(400, null, this.ErrorMessage); ;
        }
    }
}
