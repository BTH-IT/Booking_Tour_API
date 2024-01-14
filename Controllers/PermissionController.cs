﻿using Microsoft.AspNetCore.Mvc;
using BookingApi.DTO;
using BookingApi.Interfaces;
using BookingApi.Services.Interfaces;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _destinationService;

        public PermissionController(IPermissionService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _destinationService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _destinationService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] PermissionRequestDTO item)
        {
            var result = await _destinationService.Insert(item);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PermissionRequestDTO item)
        {
            var result = await _destinationService.Update(item);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _destinationService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
