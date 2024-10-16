using AutoMapper;
using Booking.API.Entities;
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

			CreateMap<TourBookingRoom, TourBookingRoomResponseDTO>().ReverseMap();
			CreateMap<TourBookingRoomRequestDTO, TourBookingRoom>();

			CreateMap<Traveller, TravellerDTO>().ReverseMap();
		}
	}
}
