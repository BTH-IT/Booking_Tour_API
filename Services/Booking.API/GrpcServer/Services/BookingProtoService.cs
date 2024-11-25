using AutoMapper;
using Booking.API.Entities;
using Booking.API.GrpcServer.Protos;
using Booking.API.Repositories.Interfaces;
using EventBus.IntergrationEvents.Events;
using Grpc.Core;
using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using ILogger = Serilog.ILogger;
namespace Booking.API.GrpcServer.Services
{
    public class BookingProtoService : BookingGrpcService.BookingGrpcServiceBase
    {
        private readonly IBookingRoomRepository bookingRoomRepository;
        private readonly IDetailBookingRoomRepository detailBookingRoomRepository;
        private readonly IBookingTourRepository tourRepository;
        private ILogger _logger;
        private readonly IPublishEndpoint publishEndpoint;  
        private readonly IMapper mapper;
        
        public BookingProtoService(IBookingRoomRepository bookingRoomRepository,
            IDetailBookingRoomRepository detailBookingRoomRepository,
            IBookingTourRepository tourRepository,
            ILogger logger,
            IPublishEndpoint publishEndpoint,
            IMapper mapper
           )
        {
            this.bookingRoomRepository = bookingRoomRepository;
            this.detailBookingRoomRepository = detailBookingRoomRepository;
            this.tourRepository = tourRepository;
            this._logger = logger;  
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        #region booking_room
        public override async Task<CheckRoomsIsBookedResponse> CheckRoomsIsBooked(CheckRoomsIsBookedRequest request, ServerCallContext context)
        {
            _logger.Information($"START - BookingProtoService - CheckRoomsIsBooked");
            var resposne = new CheckRoomsIsBookedResponse()
            {
                Result = true,
                Message = "Các phòng đều trống"
            };
            var dateStart = request.CheckIn.ToDateTime();
            var dateEnd = request.CheckOut.ToDateTime();    

            var bookingRoomsByDate = await bookingRoomRepository.FindByCondition(c=>
             c.CheckIn <= dateEnd && c.CheckOut >= dateStart,false,c=>c.DetailBookingRooms).ToListAsync();

            foreach(var item in request.RoomIds)
            {
                if(bookingRoomsByDate.Any(c=>c.DetailBookingRooms.Any(e=>e.RoomId.Equals(item))))
                {
                    resposne.Message = $"Phòng với id :{item} đã được đặt trong khoảng thời gian trên ";
                    resposne.Result = false ;
                }    
            }
            _logger.Information($"END - BookingProtoService - CheckRoomsIsBooked");

            return resposne;
        }

        public override async Task<BookingRoomResponse> CreateBookingRoom(CreateBookingRoomRequest request, ServerCallContext context)
        {
            _logger.Information($"START - BookingProtoService - CreateBookingRoom");

            double totalPrice = 0;
            int numberOfGuests = 0;
            foreach(var item in request.BookingRoomDetails)
            {
                totalPrice += item.Price;
                numberOfGuests += (item.Adult + item.Children);
            }
            var newBookingRoom = new BookingRoom()
            {
                UserId = request.UserId,
                CheckIn = request.CheckIn.ToDateTime(),
                CheckOut = request.CheckOut.ToDateTime(),
                PriceTotal = totalPrice,    
                NumberOfPeople = numberOfGuests,   
                CreatedAt = DateTime.Now,
                Status = request.Status,
            };
            var bookingRoomresult = await bookingRoomRepository.CreateAsync(newBookingRoom);
            if (bookingRoomresult <= 0) return new BookingRoomResponse()
            {
                BookingRoomId = -1
            };
            var newBookingRoomDetails = new List<DetailBookingRoom>();
            foreach(var bookingDetail in request.BookingRoomDetails)
            {
                newBookingRoomDetails.Add(new DetailBookingRoom()
                {
                    BookingId = bookingRoomresult,
                    RoomId = bookingDetail.RoomId,
                    Price = bookingDetail.Price,    
                    Adults = bookingDetail.Adult,   
                    Children = bookingDetail.Children,  
                    CreatedAt = DateTime.Now,
                });
            }
            var detailBookingRoomResult = await detailBookingRoomRepository.CreateListAsync(newBookingRoomDetails);
            _logger.Information($"END - BookingProtoService - CreateBookingRoom");

            var bookingRoom = await bookingRoomRepository.GetBookingRoomByIdAsync(bookingRoomresult);
            await publishEndpoint.Publish(new BookingRoomEvent
            {
                Id = Guid.NewGuid(),
                Data = mapper.Map<BookingRoomResponseDTO>(bookingRoom),
                Type = "CREATE"
            });
            return new BookingRoomResponse()
            {
                BookingRoomId =  bookingRoomresult,
            };
        }
  
        public override async Task<DeleteBookingRoomResponse> DeleteBookingRoom(DeleteBookingRoomRequest request, ServerCallContext context)
        {
            _logger.Information($"START - BookingProtoService - DeleteBookingRoom");

            var bookingRoom = await bookingRoomRepository.GetBookingRoomByIdAsync(request.BookingRoomId);
            if (bookingRoom == null) return new DeleteBookingRoomResponse() { Result = false };

            await bookingRoomRepository.DeleteBookingRoomAsync(request.BookingRoomId);
            if(bookingRoom.DetailBookingRooms != null)
            {
                foreach (var item in bookingRoom.DetailBookingRooms)
                {
                    await detailBookingRoomRepository.DeleteDetailBookingRoomAsync(item.Id);
                }
            }
            _logger.Information($"END - BookingProtoService - DeleteBookingRoom");
            await publishEndpoint.Publish(new BookingRoomEvent
            {
                Id = Guid.NewGuid(),
                Data = mapper.Map<BookingRoomResponseDTO>(bookingRoom),
                Type = "DELETE"
            });
            return new DeleteBookingRoomResponse()
            {
                Result = true   
            };
        }

        #endregion booking_room

        #region booking_tour
        public override async Task<BookingTourResponse> CreateBookingTour(CreateBookingTourRequest request, ServerCallContext context)
        {
            var newBookingTour = new BookingTour()
            {
                UserId = request.UserId,
                ScheduleId = request.ScheduleId,
                Seats = request.Seats,
                IsTip = request.IsTip,
                IsEntranceTicket = request.IsEntranceTicket,
                Status = request.Status,
                PriceTotal = request.PriceTotal,
                CreatedAt = DateTime.Now,
                TravellerList = new List<Traveller>()
            };
            foreach (var item in request.TravellerDetail)
            {
                newBookingTour.TravellerList.Add(new Traveller
                {
                    Gender = item.Gender,
                    Age =  sbyte.Parse(item.Age.ToString()),
                    Fullname  = item.FullName,
                    Phone = item.Phone,
                });
            }
            var bookingTourId  = await tourRepository.CreateAsync(newBookingTour);
            var bookingTour = await tourRepository.GetBookingTourByIdAsync(bookingTourId);
            await publishEndpoint.Publish(new BookingTourEvent
            {
                Id = Guid.NewGuid(),
                Data = mapper.Map<BookingTourCustomResponseDTO>(bookingTour),
                Type = "CREATE"
            });
            return new BookingTourResponse()
            {
                BookingTourId =bookingTourId,
            };
        }

        public override async Task<DeleteBookingTourResponse> DeleteBookingTour(DeleteBookingTourRequest request, ServerCallContext context)
        {
            var bookingTour = await tourRepository.GetBookingTourByIdAsync(request.BookingTourId);
            if (bookingTour == null)
            {
                return new DeleteBookingTourResponse() { Result = false };
            }
            await tourRepository.DeleteBookingTourAsync(bookingTour.Id);
            await publishEndpoint.Publish(new BookingTourEvent
            {
                Id = Guid.NewGuid(),
                Data = mapper.Map<BookingTourCustomResponseDTO>(bookingTour),
                Type = "DELETE"
            });
            return new DeleteBookingTourResponse
            {
                Result = true
            };
        }
        #endregion
    }
}
