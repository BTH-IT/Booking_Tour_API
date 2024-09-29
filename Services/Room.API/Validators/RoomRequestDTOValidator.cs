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

			RuleFor(room => room.Type)
				.IsInEnum()
				.WithMessage("Invalid room type");

			RuleFor(room => room.BedType)
				.IsInEnum()
				.WithMessage("Invalid bed type");

			RuleFor(room => room.Rate)
				.GreaterThanOrEqualTo(0)
				.WithMessage("Rate must be a positive value or zero");

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

			RuleFor(room => room.Size)
				.GreaterThan(0)
				.WithMessage("Size must be greater than 0");

			RuleFor(dto => dto.RoomAmenities)
					   .Must(roomAmenities =>
						   roomAmenities == null ||
						   roomAmenities.All(ra => ra.Title.Length <= 1000))
					   .WithMessage("Each room amenity title must not exceed 1000 characters");

			RuleFor(dto => dto.HotelAmenities)
				.Must(hotelAmenities =>
					hotelAmenities == null ||
					hotelAmenities.All(ha => ha.Title.Length <= 1000))
				.WithMessage("Each hotel amenity title must not exceed 1000 characters");

			RuleFor(room => room.CreatedAt)
				.GreaterThanOrEqualTo(DateTime.Today)
				.WithMessage("CreatedAt cannot be before today");

			RuleFor(room => room.UpdatedAt)
				.GreaterThanOrEqualTo(room => room.CreatedAt)
				.When(room => room.UpdatedAt.HasValue)
				.WithMessage("UpdatedAt cannot be before CreatedAt");

			RuleFor(room => room.HotelId)
				.GreaterThan(0)
				.WithMessage("Hotel ID must be greater than 0");
		}
	}
}
