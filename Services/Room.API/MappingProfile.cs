using AutoMapper;
using Google.Protobuf.WellKnownTypes;
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
