using FluentValidation;
using Shared.DTOs;
using Tour.API.Entities;

namespace Tour.API.Validators
{
    public class TourValidator : AbstractValidator<TourRequestDTO>
    {
        public TourValidator()
        {
            RuleFor(tour => tour.Name)
                .NotEmpty().WithMessage("Tên tour là bắt buộc.")
                .Length(1, 255).WithMessage("Tên tour phải từ 1 đến 255 ký tự.");

            RuleFor(tour => tour.MaxGuests)
                .GreaterThan(0).WithMessage("Số khách tối đa phải lớn hơn 0.");

            RuleFor(tour => tour.Price)
                .GreaterThan(0).WithMessage("Giá tour phải lớn hơn 0.");

            RuleFor(tour => tour.DateFrom)
                .NotEmpty().WithMessage("Ngày bắt đầu là bắt buộc.")
                .LessThan(tour => tour.DateTo).WithMessage("Ngày bắt đầu phải trước ngày kết thúc.");

            RuleFor(tour => tour.DateTo)
                .NotEmpty().WithMessage("Ngày kết thúc là bắt buộc.");

            RuleFor(tour => tour.Rate)
                .InclusiveBetween(0, 5).WithMessage("Đánh giá phải trong khoảng từ 0 đến 5.");

            RuleFor(tour => tour.Video)
                .MaximumLength(1000).WithMessage("URL video không vượt quá 1000 ký tự.");
        }
    }
}
