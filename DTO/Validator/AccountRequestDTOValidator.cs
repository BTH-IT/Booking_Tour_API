using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class AccountRequestDTOValidator : AbstractValidator<AccountRequestDTO>
    {
        public AccountRequestDTOValidator()
        {
            RuleFor(account => account.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(account => account.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
