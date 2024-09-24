using FluentValidation;
using Shared.DTOs;

namespace Tour.API.Validators
{
    public class TourRequestDTOValidator : AbstractValidator<TourRequestDTO>
    {
        public TourRequestDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên tour là bắt buộc.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");
            // Thêm các quy tắc khác ở đây...
        }
    }

}
