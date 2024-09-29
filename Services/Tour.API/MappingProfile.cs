using AutoMapper;
using Shared.DTOs;
using Tour.API.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Ánh xạ giữa DestinationEntity và DestinationResponseDTO
        CreateMap<DestinationEntity, DestinationResponseDTO>().ReverseMap();
        CreateMap<DestinationRequestDTO, DestinationEntity>();

        // Ánh xạ giữa TourEntity và TourResponseDTO
        CreateMap<TourEntity, TourResponseDTO>().ReverseMap();

        // Ánh xạ giữa TourRequestDTO và TourEntity
        CreateMap<TourRequestDTO, TourEntity>()
            .ForMember(dest => dest.DayList, opt => opt.MapFrom(src => src.DayList))
            .ForMember(dest => dest.ReviewList, opt => opt.MapFrom(src =>
                src.ReviewList.Select(review => new Tour.API.Entities.Review
                {
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    TourId = 0,
                    UserId = 0
                })));

        // Ánh xạ giữa Schedule và ScheduleResponseDTO
        CreateMap<Schedule, ScheduleResponseDTO>().ReverseMap();
        CreateMap<ScheduleRequestDTO, Schedule>();

        // Ánh xạ giữa Review (Entities.Review) và Review (DTOs.Review)
        CreateMap<Tour.API.Entities.Review, Shared.DTOs.Review>().ReverseMap();
    }
}
