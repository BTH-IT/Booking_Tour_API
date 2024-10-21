using AutoMapper;
using Room.API.Entities;
using Room.API.GrpcServer.Protos;
using Shared.DTOs;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Hotel, HotelResponseDTO>()
			.ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.ReviewList))
			.ForMember(dest => dest.HotelRules, opt => opt.MapFrom(src => src.HotelRulesList))
			.ForMember(dest => dest.HotelAmenities, opt => opt.MapFrom(src => src.HotelAmenitiesList))
			.ReverseMap(); 
		CreateMap<HotelRequestDTO, Hotel>()
			.ForMember(dest => dest.HotelRulesList, opt => opt.MapFrom(src => src.HotelRules))
			.ForMember(dest => dest.HotelAmenitiesList, opt => opt.MapFrom(src => src.HotelAmenities))
			.ReverseMap();

		CreateMap<RoomEntity, RoomResponseDTO>()
			.ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.ReviewList))
			.ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImagesList))
			.ForMember(dest => dest.RoomAmenities, opt => opt.MapFrom(src => src.RoomAmenitiesList))
			.ReverseMap();
		CreateMap<RoomRequestDTO, RoomEntity>()
			.ForMember(dest => dest.ImagesList, opt => opt.MapFrom(src => src.Images))
			.ForMember(dest => dest.RoomAmenitiesList, opt => opt.MapFrom(src => src.RoomAmenities))
			.ReverseMap();

		CreateMap<ReviewHotelDTO, ReviewHotel>();
		CreateMap<ReviewHotel, ReviewHotelDTO>();
		CreateMap<ReviewRoomDTO, ReviewRoom>();
		CreateMap<ReviewRoom, ReviewRoomDTO>();
		CreateMap<HotelResponse, HotelResponseDTO>().ReverseMap();
		CreateMap<RoomResponse,RoomResponseDTO>().ReverseMap();
		CreateMap<RoomEntity, RoomResponse>()
			.ForMember(dest=>dest.Hotel, opt => opt.MapFrom(src=>src.Hotel))
            .ReverseMap();
		CreateMap<Hotel, HotelResponse>().ReverseMap();
	}
}