using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class ScheduleRequestDTOValidator : AbstractValidator<ScheduleRequestDTO>
    {
        public ScheduleRequestDTOValidator()
        {
            RuleFor(bt => bt.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(bt => bt.TourId).NotEmpty().WithMessage("Name is required");
            RuleFor(bt => bt.DateStart).NotEmpty().WithMessage("IsWifi is required");
            RuleFor(bt => bt.DateEnd).NotEmpty().WithMessage("Detail is required");
            RuleFor(bt => bt.AvailableSeats).NotEmpty().WithMessage("Expect is required");
        }
    }
}
