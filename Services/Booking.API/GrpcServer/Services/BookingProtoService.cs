using Booking.API.Entities;
using Booking.API.GrpcServer.Protos;
using Booking.API.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Booking.API.GrpcServer.Services
{
    public class BookingProtoService : BookingGrpcService.BookingGrpcServiceBase
    {
        private IBookingRoomRepository bookingRoomRepository;
        private IDetailBookingRoomRepository detailBookingRoomRepository;
        
        public BookingProtoService(IBookingRoomRepository bookingRoomRepository,
            IDetailBookingRoomRepository detailBookingRoomRepository)
        {
            this.bookingRoomRepository = bookingRoomRepository;
            this.detailBookingRoomRepository = detailBookingRoomRepository; 
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
