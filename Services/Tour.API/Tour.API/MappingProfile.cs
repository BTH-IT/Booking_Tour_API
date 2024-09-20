using AutoMapper;
using Shared.DTOs;
using Tour.API.Entities;

namespace Tour.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<DestinationEntity, DestinationResponseDTO>().ReverseMap();
            CreateMap<DestinationRequestDTO, DestinationEntity>();

            CreateMap<TourEntity, TourResponseDTO>().ReverseMap();
            CreateMap<TourRequestDTO, TourEntity>();

            CreateMap<Schedule, ScheduleResponseDTO>().ReverseMap();
            CreateMap<ScheduleRequestDTO, Schedule>();

            CreateMap<TourRequestDTO, TourEntity>()
            .ForMember(dest => dest.DayList, opt => opt.MapFrom(src =>
                src.DayList.Select(date => new Day { Date = date })))
            .ForMember(dest => dest.ReviewList, opt => opt.MapFrom(src =>
                src.ReviewList.Select(review => new Entities.Review
                {
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                    TourId = 0, 
                    UserId = 0 
                })));
            CreateMap<Entities.Review, Entities.Review>() 
            .ForMember(dest => dest.TourId, opt => opt.Ignore()) 
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
