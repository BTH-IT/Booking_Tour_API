using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class PermissionRequestDTOValidator : AbstractValidator<PermissionRequestDTO>
    {
        public PermissionRequestDTOValidator()
        {
            RuleFor(bt => bt.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(bt => bt.Status).NotEmpty().WithMessage("Description is required");
        }
    }
}
