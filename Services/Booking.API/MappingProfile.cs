using AutoMapper;
using Booking.API.Entities;
using Booking.API.GrpcClient.Protos;
using Google.Protobuf.WellKnownTypes;
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
            CreateMap<BookingTourCustomResponseDTO, BookingTour>()
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
            CreateMap<UserResponse, UserResponseDTO>()
				.ForMember(dest => dest.BirthDate, opt =>
				{
					opt.PreCondition(src => src.BirthDate != null);
					opt.MapFrom(src => src.BirthDate.ToDateTime());
				});
            CreateMap<HotelResponse, HotelResponseDTO>().ReverseMap();
            CreateMap<RoomResponse, RoomResponseDTO>().ReverseMap();
			CreateMap<ReviewResponse, ReviewRoomDTO>();
			CreateMap<ScheduleResponse, ScheduleResponseDTO>()
				.ForMember(dest => dest.DateStart, opt => opt.MapFrom(src => src.DateStart.ToDateTime()))
				.ForMember(dest => dest.DateEnd, opt => opt.MapFrom(src => src.DateEnd.ToDateTime()));

            CreateMap<ScheduleResponse, ScheduleCustomResponseDTO>()
                .ForMember(dest => dest.DateStart, opt => opt.MapFrom(src => src.DateStart.ToDateTime()))
                .ForMember(dest => dest.DateEnd, opt => opt.MapFrom(src => src.DateEnd.ToDateTime()));
            CreateMap<TourResponse, TourCustomResponseDTO>()
                .ForMember(dest => dest.DateFrom, opt => opt.MapFrom(src => src.DateFrom.ToDateTime()))
                .ForMember(dest => dest.DateTo, opt => opt.MapFrom(src => src.DateTo.ToDateTime())); 
        }
	}
}
