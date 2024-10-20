using Booking.API.Entities;
using Booking.API.GrpcServer.Protos;
using Booking.API.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Booking.API.GrpcServer.Services
{
    public class BookingProtoService : BookingGrpcService.BookingGrpcServiceBase
    {
        private readonly IBookingRoomRepository bookingRoomRepository;
        private readonly IDetailBookingRoomRepository detailBookingRoomRepository;
        private readonly IBookingTourRepository tourRepository;
        private readonly ITourBookingRoomRepository tourBookingRoomRepository;
        
        public BookingProtoService(IBookingRoomRepository bookingRoomRepository,
            IDetailBookingRoomRepository detailBookingRoomRepository,
            IBookingTourRepository tourRepository,
            ITourBookingRoomRepository tourBookingRoomRepository)
        {
            this.bookingRoomRepository = bookingRoomRepository;
            this.detailBookingRoomRepository = detailBookingRoomRepository;
            this.tourRepository = tourRepository;
            this.tourBookingRoomRepository = tourBookingRoomRepository;
        }

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

            var bookingToursByDate = await tourRepository.FindByCondition(c =>
             c.DateStart <= dateEnd && c.DateEnd >= dateStart, false, c => c.TourBookingRooms).ToListAsync();

            foreach(var item in request.RoomIds)
            {
                if(bookingRoomsByDate.Any(c=>c.DetailBookingRooms.Any(e=>e.RoomId.Equals(item))))
                {
                    resposne.Message = $"Phòng với id :{item} đã được đặt trong khoảng thời gian trên ";
                    resposne.Result = false ;
                }    
                if(bookingToursByDate.Any(c=>c.TourBookingRooms.Any(e=>e.RoomId.Equals(item))))
                {
                    resposne.Message = $"Phòng với id :{item} đã được đặt trong khoảng thời gian trên ";
                    resposne.Result = false;
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
            foreach(var item in bookingRoom.DetailBookingRooms)
            {
                await detailBookingRoomRepository.DeleteDetailBookingRoomAsync(item.BookingId);  
            }
            return new DeleteBookingRoomResponse()
            {
                Result = true   
            };
        }
    }
}
