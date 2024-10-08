using FluentValidation;
using Shared.DTOs;

namespace Room.API.Validators
{
	public class RoomRequestDTOValidator : AbstractValidator<RoomRequestDTO>
	{
		public RoomRequestDTOValidator()
		{
			RuleFor(room => room.Name)
				.NotEmpty()
				.WithMessage("Room name is required");

			RuleFor(room => room.Price)
				.GreaterThanOrEqualTo(0)
				.WithMessage("Price must be a positive value or zero");

			RuleFor(room => room.Detail)
				.MaximumLength(1000)
				.WithMessage("Details must not exceed 1000 characters");

			RuleFor(room => room.MaxGuests)
				.GreaterThan(0)
				.WithMessage("MaxGuests must be greater than 0");

			RuleFor(room => room.HotelId)
				.GreaterThan(0)
				.WithMessage("Hotel ID must be greater than 0");
		}
	}
}
