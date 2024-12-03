using AutoMapper;
using Google.Protobuf.WellKnownTypes;
<<<<<<< HEAD
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
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.ReviewList.ToList()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImagesList.ToList()))
            .ForMember(dest => dest.RoomAmenities, opt => opt.MapFrom(src => src.RoomAmenitiesList.ToList()))
            .ForMember(dest=>dest.Hotel, opt => opt.MapFrom(src=>src.Hotel))
            .ReverseMap();

        CreateMap<Hotel, HotelResponse>().ReverseMap();
        CreateMap<ReviewRoom, ReviewResponse>()
			.ForMember(dest=>dest.CreatedAt,opt => opt.MapFrom(src=>Timestamp.FromDateTime(src.CreatedAt.Value.ToUniversalTime())))
			.ReverseMap();
    }
}
=======
using Newtonsoft.Json;
using Room.API.Entities;
using Shared.DTOs;

namespace Room.API
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Hotel, HotelResponseDTO>()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<ReviewHotelDTO>>(src.Reviews)))
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelRulesDTO>>(src.HotelRules)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelAmenitiesDTO>>(src.HotelAmenities)))
				.ReverseMap()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Reviews)))
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelRules)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelAmenities)));

			CreateMap<HotelRequestDTO, Hotel>()
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelRules)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelAmenities)))
				.ReverseMap()
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelRulesDTO>>(src.HotelRules)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelAmenitiesDTO>>(src.HotelAmenities)));

			CreateMap<RoomEntity, RoomResponseDTO>()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<ReviewRoomDTO>>(src.Reviews)))
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<RoomAmenitiesDTO>>(src.RoomAmenities)))
				.ForMember(dest => dest.Images, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<ImagesDTO>>(src.Images)))
				.ForMember(dest => dest.Video, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<VideosDTO>(src.Video)))
				.ReverseMap()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Reviews)))
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.RoomAmenities)))
				.ForMember(dest => dest.Images, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Images)))
				.ForMember(dest => dest.Video, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Video)));

			CreateMap<RoomRequestDTO, RoomEntity>()
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.RoomAmenities)))
				.ForMember(dest => dest.Images, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Images)))
				.ForMember(dest => dest.Video, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Video)))
			.ReverseMap()
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<RoomAmenitiesDTO>>(src.RoomAmenities)))
				.ForMember(dest => dest.Images, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<Image>>(src.Images)))
				.ForMember(dest => dest.Video, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<Video>(src.Video)));


			CreateMap<ReviewHotelDTO, ReviewHotel>();
			CreateMap<ReviewHotel, ReviewHotelDTO>();
			CreateMap<ReviewRoomDTO, ReviewRoom>();
			CreateMap<ReviewRoom, ReviewRoomDTO>();
		}
	}
}
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
