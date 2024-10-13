using AutoMapper;
using Booking.API.Entities;
using Shared.DTOs;

namespace Booking.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<BookingRoomResponseDTO, BookingRoom>();

        }
    }
}
