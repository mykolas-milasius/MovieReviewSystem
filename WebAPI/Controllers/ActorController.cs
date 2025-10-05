using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateActor([FromBody] CreateActorDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
            {
                return UnprocessableEntity("First name and last name are required.");
            }

            var result = await _actorService.CreateActorAsync(request);
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
        public async Task<IActionResult> UpdateActor([FromBody] UpdateActorDTO request)
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

            var result = await _actorService.UpdateActorAsync(request);

            if (result == null)
            {
                return NotFound($"Actor with id {request.Id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteActor([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

            var deleted = await _actorService.DeleteActorAsync(id);

            if (!deleted)
            {
                return NotFound($"Actor with id {id} not found.");
            }

            return NoContent();
        }
    }
}
