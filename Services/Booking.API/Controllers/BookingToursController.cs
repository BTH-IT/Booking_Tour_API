using AutoMapper;
using Booking.API.Repositories.Interfaces;
using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingToursController : ControllerBase
    {
    }
}
