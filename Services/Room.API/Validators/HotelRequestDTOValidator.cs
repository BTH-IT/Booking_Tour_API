using FluentValidation;
using Shared.DTOs;

namespace Room.API.Validators
{
	public class HotelRequestDTOValidator : AbstractValidator<HotelRequestDTO>
	{
		public HotelRequestDTOValidator()
		{
			RuleFor(hotel => hotel.Name)
				.NotEmpty()
				.WithMessage("Hotel name is required");

			RuleFor(hotel => hotel.Location)
				.NotEmpty()
				.WithMessage("Hotel location is required");

			RuleFor(hotel => hotel.Description)
				.NotNull()
				.MaximumLength(500)
				.WithMessage("Description must not exceed 500 characters and cannot be null");

			RuleFor(hotel => hotel.ContactInfo)
				.NotEmpty()
				.WithMessage("Contact information is required");

			RuleFor(hotel => hotel.Rate)
				.InclusiveBetween(1, 5)
				.WithMessage("Star rating must be between 1 and 5");

			RuleFor(hotel => hotel.Description)
				.MaximumLength(500)
				.WithMessage("Description must not exceed 500 characters");


			RuleFor(hotel => hotel.CreatedAt)
				.GreaterThanOrEqualTo(DateTime.Today)
				.WithMessage("CreatedAt cannot be before today");

			RuleFor(hotel => hotel.UpdatedAt)
				.GreaterThanOrEqualTo(hotel => hotel.CreatedAt)
				.When(hotel => hotel.UpdatedAt.HasValue)
				.WithMessage("UpdatedAt cannot be before CreatedAt");
		}
	}
}
