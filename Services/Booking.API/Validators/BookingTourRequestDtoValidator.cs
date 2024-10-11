using FluentValidation;
using Shared.DTOs;

namespace Booking.API.Validators
{
    public class BookingTourRequestDtoValidator : AbstractValidator<BookingTourRequestDTO>
    {
        public BookingTourRequestDtoValidator() { }
    }
}
