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

			RuleFor(hotel => hotel.LocationCode)
				.NotEmpty()
				.WithMessage("Hotel LocationCode is required");

			RuleFor(hotel => hotel.Description)
				.NotNull()
				.MaximumLength(1000)
				.WithMessage("Description must not exceed 1000 characters and cannot be null");

			RuleFor(hotel => hotel.ContactInfo)
				.NotEmpty()
				.WithMessage("Contact information is required");
		}
	}
}
