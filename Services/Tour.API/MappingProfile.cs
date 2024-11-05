using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Shared.DTOs;
using Tour.API.Entities;
using Tour.API.GrpcClient.Protos;
using Tour.API.GrpcServer.Protos;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		// Ánh xạ giữa DestinationEntity và DestinationResponseDTO
		CreateMap<DestinationEntity, DestinationResponseDTO>().ReverseMap();
		CreateMap<DestinationRequestDTO, DestinationEntity>().ReverseMap();

		// Ánh xạ giữa TourEntity và TourResponseDTO
		CreateMap<TourEntity, TourResponseDTO>()
			.ReverseMap();

		// Ánh xạ giữa TourRequestDTO và TourEntity
		CreateMap<TourRequestDTO, TourEntity>()
			.ForMember(dest => dest.DayList, opt => opt.MapFrom(src => src.DayList))
			.ReverseMap();

        // Ánh xạ giữa Schedule và ScheduleResponseDTO
        CreateMap<Schedule, ScheduleResponseDTO>().ReverseMap();
		CreateMap<ScheduleRequestDTO, Schedule>();

		// Ánh xạ giữa Review (Entities.Review) và Review (DTOs.Review)
		CreateMap<Review, ReviewTourDTO>().ReverseMap();

		// Ánh xạ giữa Video và VideoRoom
		CreateMap<Review, ReviewTourDTO>().ReverseMap();
		CreateMap<ReviewTourDTO, Review > ().ReverseMap();

		// Ánh xạ giữa Grpc và Entities
		CreateMap<DestinationEntity, DestinationResponse>();

		CreateMap<TourEntity, TourResponse>()
			.ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination))
			.ForMember(dest => dest.DateFrom, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DateFrom.ToUniversalTime()) ))
			.ForMember(dest => dest.DateTo, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DateTo.ToUniversalTime())));

		CreateMap<Schedule, ScheduleResponse>()
			.ForMember(dest =>dest.Tour,opt => opt.MapFrom(src=>src.Tour))
			.ForMember(dest => dest.DateStart,opt=>opt.MapFrom(src=> Timestamp.FromDateTime(src.DateStart.Value.ToUniversalTime())))
			.ForMember(dest => dest.DateEnd,opt=>opt.MapFrom(src=> Timestamp.FromDateTime(src.DateEnd.Value.ToUniversalTime())));
        CreateMap<HotelResponse, HotelResponseDTO>().ReverseMap();
        CreateMap<RoomResponse, RoomResponseDTO>().ReverseMap();
        CreateMap<HotelResponse, HotelResponseDTO>().ReverseMap();
        CreateMap<ReviewResponse, ReviewRoomDTO>().ReverseMap();

    }
}
