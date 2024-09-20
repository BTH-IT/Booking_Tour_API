using FluentValidation;
using Shared.DTOs;
using Tour.API.Entities;

namespace Tour.API.Validators
{
    public class DestinationValidator : AbstractValidator<DestinationRequestDTO>
    {
        public DestinationValidator()
        {
            RuleFor(destination => destination.Name)
                .NotEmpty().WithMessage("Tên điểm đến là bắt buộc.")
                .Length(1, 100).WithMessage("Tên điểm đến phải từ 1 đến 100 ký tự.");

            RuleFor(destination => destination.Url)
                .NotEmpty().WithMessage("URL là bắt buộc.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
                .WithMessage("URL không hợp lệ.");

            RuleFor(destination => destination.Description)
                .MaximumLength(500).WithMessage("Mô tả không vượt quá 500 ký tự.");
        }
    }
}
