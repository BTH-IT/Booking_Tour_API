using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class AuthRegisterDTOValidator : AbstractValidator<AuthRegisterDTO>
    {
        public AuthRegisterDTOValidator()
        {
            RuleFor(l => l.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(l => l.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(l => l.Fullname).NotEmpty().WithMessage("Fullname is required");
            RuleFor(l => l.BirthDate).NotEmpty().WithMessage("BirthDate is required");
            RuleFor(l => l.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(l => l.Phone).NotEmpty().WithMessage("Phone is required");
            RuleFor(l => l.Gender).NotEmpty().WithMessage("Gender is required");
        }
    }
}
