using FluentValidation;
using Shared.DTOs;
using Tour.API.Entities;

namespace Tour.API.Validators
{
    public class ScheduleValidator : AbstractValidator<ScheduleRequestDTO>
    {
        public ScheduleValidator()
        {
            RuleFor(schedule => schedule.DateStart)
                .NotEmpty().WithMessage("Ngày bắt đầu là bắt buộc.");

            RuleFor(schedule => schedule.DateEnd)
                .NotEmpty().WithMessage("Ngày kết thúc là bắt buộc.")
                .GreaterThan(schedule => schedule.DateStart)
                .WithMessage("Ngày kết thúc phải lớn hơn ngày bắt đầu.");

            RuleFor(schedule => schedule.AvailableSeats)
                .GreaterThan(0).WithMessage("Số ghế còn lại phải lớn hơn 0.");

            RuleFor(schedule => schedule.TourId)
                .GreaterThan(0).WithMessage("Mã tour phải hợp lệ.");
        }
    }
}
