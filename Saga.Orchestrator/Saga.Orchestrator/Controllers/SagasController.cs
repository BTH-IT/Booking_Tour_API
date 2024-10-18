using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SagasController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TestAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(userId);
        }
    }
}
