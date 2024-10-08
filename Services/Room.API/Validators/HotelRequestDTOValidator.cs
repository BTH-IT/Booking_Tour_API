﻿using FluentValidation;
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

			RuleFor(hotel => hotel.Description)
				.MaximumLength(500)
				.WithMessage("Description must not exceed 500 characters");

			RuleFor(dto => dto.HotelRules)
				.Must(HotelRules =>
					HotelRules == null ||
					HotelRules.All(ha => ha.Title.Length <= 1000))
				.WithMessage("Each hotel rules title must not exceed 1000 characters");

			RuleFor(dto => dto.HotelAmenities)
				.Must(hotelAmenities =>
					hotelAmenities == null ||
					hotelAmenities.All(ha => ha.Title.Length <= 1000))
				.WithMessage("Each hotel amenity title must not exceed 1000 characters");
		}
	}
}
