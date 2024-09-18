using FluentValidation;
using Shared.DTOs;

namespace BookingApi.DTO.Validator
{
    public class UserRequestDTOValidator : AbstractValidator<UserRequestDTO>
    {
        public UserRequestDTOValidator() {
            RuleFor(x => x.Fullname).NotEmpty().WithMessage("Fullname is required");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("BirthDate is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required");
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("AccountId is required");
        }
    }
}
