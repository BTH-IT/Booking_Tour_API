using BookingApi.Models;
using FluentValidation;

namespace BookingApi.DTO.Validator
{
    public class TourRequestDTOValidator : AbstractValidator<TourRequestDTO>
    {
        public TourRequestDTOValidator()
        {
            RuleFor(bt => bt.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(bt => bt.IsWifi).NotEmpty().WithMessage("IsWifi is required");
            RuleFor(bt => bt.Detail).NotEmpty().WithMessage("Detail is required");
            RuleFor(bt => bt.Expect).NotEmpty().WithMessage("Expect is required");
            RuleFor(bt => bt.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(bt => bt.DateFrom).NotEmpty().WithMessage("DateFrom is required");
            RuleFor(bt => bt.DateTo).NotEmpty().WithMessage("DateTo is required");
            RuleFor(bt => bt.Rate).NotEmpty().WithMessage("Rate is required");
            RuleFor(bt => bt.Video).NotEmpty().WithMessage("Video is required");
            RuleFor(bt => bt.SalePercent).NotEmpty().WithMessage("SalePercent is required");
            RuleFor(bt => bt.PriceExcludeList).NotEmpty().WithMessage("PriceExcludeList is required");
            RuleFor(bt => bt.PriceIncludeList).NotEmpty().WithMessage("PriceIncludeList is required");
            RuleFor(bt => bt.ImageList).NotEmpty().WithMessage("ImageList is required");
            RuleFor(bt => bt.DayList).NotEmpty().WithMessage("DayList is required");
            RuleFor(bt => bt.DestinationId).NotEmpty().WithMessage("DestinationId is required");
            RuleFor(bt => bt.ReviewList).NotEmpty().WithMessage("ReviewList is required");
        }
    }
}
