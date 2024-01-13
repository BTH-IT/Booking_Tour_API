using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class AuthLoginDTOValidator : AbstractValidator<AuthLoginDTO>
    {
        public AuthLoginDTOValidator()
        {
            RuleFor(l => l.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(l => l.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
