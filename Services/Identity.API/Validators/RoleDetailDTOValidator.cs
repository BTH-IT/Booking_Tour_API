using FluentValidation;
using Shared.DTOs;

namespace BookingApi.DTO.Validator
{
    public class RoleDetailDTOValidator : AbstractValidator<RoleDetailDTO>
    {
        public RoleDetailDTOValidator()
        {
            RuleFor(detail => detail.ActionName).NotEmpty().WithMessage("ActionName is required");
        }
    }
}
