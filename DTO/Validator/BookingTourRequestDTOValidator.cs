using BookingApi.Models;
using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class BookingTourRequestDTOValidator : AbstractValidator<BookingTourRequestDTO>
    {
        public BookingTourRequestDTOValidator()
        {
            RuleFor(bt => bt.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(bt => bt.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(bt => bt.ScheduleId).NotEmpty().WithMessage("ScheduleId is required");
            RuleFor(bt => bt.Seats).NotEmpty().WithMessage("Seats is required");
            RuleFor(bt => bt.Umbrella).NotEmpty().WithMessage("Umbrella is required");
            RuleFor(bt => bt.IsCleaningFee).NotEmpty().WithMessage("IsCleaningFee is required");
            RuleFor(bt => bt.IsTip).NotEmpty().WithMessage("IsTip is required");
            RuleFor(bt => bt.IsEntranceTicket).NotEmpty().WithMessage("IsEntranceTicket is required");
            RuleFor(bt => bt.Status).NotEmpty().WithMessage("Status is required");
            RuleFor(bt => bt.PriceTotal).NotEmpty().WithMessage("PriceTotal is required");
            RuleFor(bt => bt.Coupon).NotEmpty().WithMessage("Coupon is required");
            RuleFor(bt => bt.PaymentMethod).NotEmpty().WithMessage("PaymentMethod is required");
            RuleFor(bt => bt.Travellers).NotEmpty().WithMessage("Travellers is required");
        }
    }
}
