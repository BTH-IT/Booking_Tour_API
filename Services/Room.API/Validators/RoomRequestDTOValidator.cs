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

			RuleFor(room => room.IsAvailable)
				.NotNull()
				.WithMessage("Availability status is required");

			RuleFor(room => room.Detail)
				.MaximumLength(1000)
				.WithMessage("Details must not exceed 1000 characters");

			RuleFor(room => room.Video)
				.MaximumLength(500)
				.WithMessage("Video URL must not exceed 500 characters");

			RuleFor(room => room.MaxGuests)
				.GreaterThan(0)
				.WithMessage("MaxGuests must be greater than 0");

			RuleFor(dto => dto.RoomAmenities)
					   .Must(roomAmenities =>
						   roomAmenities == null ||
						   roomAmenities.All(ra => ra.Title.Length <= 1000))
					   .WithMessage("Each room amenity title must not exceed 1000 characters");

			RuleFor(room => room.HotelId)
				.GreaterThan(0)
				.WithMessage("Hotel ID must be greater than 0");
		}
	}
}
