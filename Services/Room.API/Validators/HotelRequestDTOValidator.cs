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
<<<<<<< HEAD
				.MaximumLength(1000)
				.WithMessage("Description must not exceed 1000 characters and cannot be null");
=======
				.MaximumLength(500)
				.WithMessage("Description must not exceed 500 characters and cannot be null");
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

			RuleFor(hotel => hotel.ContactInfo)
				.NotEmpty()
				.WithMessage("Contact information is required");
		}
	}
}
