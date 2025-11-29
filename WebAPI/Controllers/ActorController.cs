using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using WebAPI.DTOs.Actor;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllActors()
        {
            var result = await _actorService.GetAllActorsAsync();

			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateActor([FromBody] CreateActorDTO request, HttpContext httpContext)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
            {
                return UnprocessableEntity("First name and last name are required.");
            }

			string userId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _actorService.CreateActorAsync(request, userId);
            return CreatedAtAction(nameof(GetActorById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActorById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0.");
            }

            var result = await _actorService.GetActorByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateActor([FromBody] UpdateActorDTO request, HttpContext httpContext)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (request.Id <= 0)
            {
                return UnprocessableEntity("Id must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
            {
                return UnprocessableEntity("First name and last name are required.");
            }

			string userId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _actorService.UpdateActorAsync(request, userId);

            if (result == null)
            {
                return NotFound($"Actor with id {request.Id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete]
		[Authorize]
		public async Task<IActionResult> DeleteActor([FromQuery] int id, HttpContext httpContext)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

			string userId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var deleted = await _actorService.DeleteActorAsync(id, userId);

            if (!deleted)
            {
                return NotFound($"Actor with id {id} not found.");
            }

            return NoContent();
        }
    }
}
