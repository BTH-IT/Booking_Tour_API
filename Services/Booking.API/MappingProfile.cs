using AutoMapper;
using Booking.API.Entities;
using Booking.API.GrpcClient.Protos;
using Newtonsoft.Json;
using Shared.DTOs;

namespace Booking.API
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<BookingRoom, BookingRoomResponseDTO>().ReverseMap();
			CreateMap<BookingRoomRequestDTO, BookingRoom>();
			CreateMap<DetailBookingRoom, DetailBookingRoomResponseDTO>().ReverseMap();
			CreateMap<DetailBookingRoomRequestDTO, DetailBookingRoom>();

			CreateMap<BookingTour, BookingTourResponseDTO>()
				.ForMember(dest => dest.Travellers, opt =>
				{
					 opt.PreCondition(src => src.Travellers != null);
					 opt.MapFrom(src => JsonConvert.DeserializeObject<List<TravellerDTO>>(src.Travellers));
				})
				.ReverseMap()
				.ForMember(dest => dest.Travellers, opt =>
				{
					opt.PreCondition(src => src.Travellers != null);
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Travellers));
				});
			CreateMap<BookingTourRequestDTO, BookingTour>()
				.ForMember(dest => dest.Travellers, opt =>
				{
					opt.PreCondition(src => src.Travellers != null);
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Travellers));
				})
				.ReverseMap()
				.ForMember(dest => dest.Travellers, opt =>
				{
					opt.PreCondition(src => src.Travellers != null);
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<TravellerDTO>>(src.Travellers));
				});


			CreateMap<Traveller, TravellerDTO>().ReverseMap();
			CreateMap<GetUserByIdResponse, UserResponseDTO>()
				.ForMember(dest => dest.BirthDate, opt =>
				{
					opt.PreCondition(src => src.BirthDate != null);
					opt.MapFrom(src => src.BirthDate.ToDateTime());
				});
            CreateMap<HotelResponse, HotelResponseDTO>().ReverseMap();
            CreateMap<RoomResponse, RoomResponseDTO>().ReverseMap();

			CreateMap<ScheduleResponse, ScheduleResponseDTO>().ReverseMap();
			CreateMap<TourResponse, TourResponseDTO>().ReverseMap();
        }
	}
}
