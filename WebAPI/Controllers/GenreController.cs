using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using WebAPI.DTOs.Genre;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return UnprocessableEntity("Genre title is required.");
            }

			string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _genreService.CreateGenreAsync(request, userId);
            return CreatedAtAction(nameof(GetGenreById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetGenreById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0.");
            }

            var result = await _genreService.GetGenreByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateGenre([FromBody] UpdateGenreDTO request)
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
                return UnprocessableEntity("Genre title is required.");
            }

			string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _genreService.UpdateGenreAsync(request, userId);

            if (result == null)
            {
                return NotFound($"Genre with id {request.Id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete]
		[Authorize]
		public async Task<IActionResult> DeleteGenre([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

			string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var deleted = await _genreService.DeleteGenreAsync(id, userId);

            if (!deleted)
            {
                return NotFound($"Genre with id {id} not found.");
            }

            return NoContent();
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllGenres()
        {
            var result = await _genreService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetMoviesByGenre")]
        public async Task<IActionResult> GetMoviesByGenreId([FromQuery] int id)
        {
            var result = await _genreService.GetMoviesByGenreIdAsync(id);
            return Ok(result);
		}
    }
}
