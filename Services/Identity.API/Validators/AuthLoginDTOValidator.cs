using FluentValidation;
using Shared.DTOs;

namespace Identity.API.Validators
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
