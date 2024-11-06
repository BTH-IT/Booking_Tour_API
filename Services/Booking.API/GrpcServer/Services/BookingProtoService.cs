using Booking.API.Entities;
using Booking.API.GrpcServer.Protos;
using Booking.API.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.GrpcServer.Services
{
    public class BookingProtoService : BookingGrpcService.BookingGrpcServiceBase
    {
        private readonly IBookingRoomRepository bookingRoomRepository;
        private readonly IDetailBookingRoomRepository detailBookingRoomRepository;
        private readonly IBookingTourRepository tourRepository;
        
        public BookingProtoService(IBookingRoomRepository bookingRoomRepository,
            IDetailBookingRoomRepository detailBookingRoomRepository,
            IBookingTourRepository tourRepository
           )
        {
            this.bookingRoomRepository = bookingRoomRepository;
            this.detailBookingRoomRepository = detailBookingRoomRepository;
            this.tourRepository = tourRepository;
        }
        #region booking_room
        public override async Task<CheckRoomsIsBookedResponse> CheckRoomsIsBooked(CheckRoomsIsBookedRequest request, ServerCallContext context)
        {
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
            return resposne;
        }

        public override async Task<BookingRoomResponse> CreateBookingRoom(CreateBookingRoomRequest request, ServerCallContext context)
        {
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
                CreatedAt = DateTime.Now
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
            return new BookingRoomResponse()
            {
                BookingRoomId =  bookingRoomresult,
            };
        }
  
        public override async Task<DeleteBookingRoomResponse> DeleteBookingRoom(DeleteBookingRoomRequest request, ServerCallContext context)
        {
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
            return new DeleteBookingTourResponse
            {
                Result = true
            };
        }
        #endregion
    }
}
