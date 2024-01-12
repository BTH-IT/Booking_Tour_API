using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class ReviewRequestDTOValidator : AbstractValidator<ReviewRequestDTO>
    {
        public ReviewRequestDTOValidator()
        {
            RuleFor(bt => bt.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(bt => bt.Content).NotEmpty().WithMessage("Content is required");
            RuleFor(bt => bt.Rating).NotEmpty().WithMessage("Rating is required");
            RuleFor(bt => bt.TourId).NotEmpty().WithMessage("TourId is required");
            RuleFor(bt => bt.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
