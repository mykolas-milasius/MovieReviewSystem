using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.DTOs.Movie;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

		[HttpGet("all")]
		public async Task<IActionResult> GetAllMovies()
		{
			var result = await _movieService.GetAllMovies();
			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDTO request, HttpContext httpContext)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return UnprocessableEntity("Movie title is required");
            }

			string userId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _movieService.CreateMovieAsync(request, userId);
            return CreatedAtAction(nameof(GetMovieById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0.");
            }

            var result = await _movieService.GetMovieByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieDTO request, HttpContext httpContext)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (request.Id <= 0)
            {
                return UnprocessableEntity("Id must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return UnprocessableEntity("Title is required");
            }

			string userId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _movieService.UpdateMovieAsync(request, userId);

            if (result == null)
            {
                return NotFound($"Movie with id {request.Id} not found.");
            }

            return Ok(result);
        }

		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> DeleteMovie([FromQuery] int id, HttpContext httpContext)
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

			var deleted = await _movieService.DeleteMovieAsync(id, userId);

            if (!deleted)
            {
                return NotFound($"Movie with id {id} not found.");
            }

            return NoContent();
        }

        [HttpGet("GenresByMovieId")]
        public async Task<IActionResult> GetGenresByMovieId([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

            var result = await _movieService.GetAllGenresByMovieId(id);

            return Ok(result);
        }

        [HttpGet("ActorsByMovieId")]
        public async Task<IActionResult> GetActorsByMovieId([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

            var result = await _movieService.GetAllActorsByMovieId(id);

            return Ok(result);
        }

        [HttpGet("ReviewsByMovieId")]
        public async Task<IActionResult> GetReviewsByMovieId([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

            var result = await _movieService.GetAllReviewsByMovieId(id);

            return Ok(result);
        }
	}
}
