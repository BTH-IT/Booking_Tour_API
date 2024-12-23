﻿using FluentValidation;
using Shared.DTOs;

namespace BookingApi.DTO.Validator
{
    public class RoleRequestDTOValidator : AbstractValidator<RoleRequestDTO>
    {
        public RoleRequestDTOValidator()
        {
            RuleFor(role => role.RoleName).NotEmpty().WithMessage("RoleName is required");
        }
    }
}
