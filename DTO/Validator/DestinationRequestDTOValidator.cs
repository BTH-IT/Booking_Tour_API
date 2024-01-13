using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class DestinationRequestDTOValidator : AbstractValidator<DestinationRequestDTO>
    {
        public DestinationRequestDTOValidator ()
        {
            RuleFor(bt => bt.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(bt => bt.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(bt => bt.Url).NotEmpty().WithMessage("Url is required");
        }
    }
}
