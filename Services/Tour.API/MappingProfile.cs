using AutoMapper;
using Newtonsoft.Json;
using Shared.DTOs;
using Tour.API.Entities;

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
		CreateMap<Tour.API.Entities.Review, Shared.DTOs.Review>().ReverseMap();

		// Ánh xạ giữa Video và VideoRoom

		CreateMap<Tour.API.Entities.Review, Shared.DTOs.Review >().ReverseMap();
		CreateMap<Shared.DTOs.Review,Tour.API.Entities.Review > ().ReverseMap();
	}
}
