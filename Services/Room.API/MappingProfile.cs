﻿using AutoMapper;
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
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<Shared.DTOs.ReviewHotel>>(src.Reviews)))
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelRulesDTO>>(src.HotelRules)))
				.ReverseMap()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Reviews)))
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelRules)));

			CreateMap<HotelRequestDTO, Hotel>()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Reviews)))
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelRules)))
				.ReverseMap()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<Shared.DTOs.ReviewHotel>>(src.Reviews)))
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelRulesDTO>>(src.HotelRules)));

			CreateMap<Hotel, HotelRulesResponseDTO>()
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelRulesDTO>>(src.HotelRules)))
				.ReverseMap()
				.ForMember(dest => dest.HotelRules, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelRules)));


			CreateMap<RoomEntity, RoomResponseDTO>()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<Shared.DTOs.ReviewRoom>>(src.Reviews)))
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<RoomAmenitiesDTO>>(src.RoomAmenities)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelAmenitiesDTO>>(src.HotelAmenities)))
				.ReverseMap()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Reviews)))
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.RoomAmenities)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelAmenities)));

			CreateMap<RoomRequestDTO, RoomEntity>()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.Reviews)))
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.RoomAmenities)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.SerializeObject(src.HotelAmenities)))
				.ReverseMap()
				.ForMember(dest => dest.Reviews, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<Shared.DTOs.ReviewRoom>>(src.Reviews)))
				.ForMember(dest => dest.RoomAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<RoomAmenitiesDTO>>(src.RoomAmenities)))
				.ForMember(dest => dest.HotelAmenities, opt =>
					opt.MapFrom(src => JsonConvert.DeserializeObject<List<HotelAmenitiesDTO>>(src.HotelAmenities)));
		}
	}
}