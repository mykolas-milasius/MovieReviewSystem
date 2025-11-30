using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.DTOs.Review;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Author))
            {
                return UnprocessableEntity("Author is required");
            }

            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _reviewService.CreateReviewAsync(request, userId);
            return CreatedAtAction(nameof(GetReviewById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0.");
            }

            var result = await _reviewService.GetReviewByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (request.Id <= 0)
            {
                return UnprocessableEntity("Id must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(request.Author))
            {
                return UnprocessableEntity("Author is required");
            }

            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _reviewService.UpdateReviewAsync(request, userId);

            if (result == null)
            {
                return NotFound($"Review with id {request.Id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteReview([FromQuery] int id)
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

            var deleted = await _reviewService.DeleteReviewAsync(id, userId);

            if (!deleted)
            {
                return NotFound($"Review with id {id} not found.");
            }

            return NoContent();
        }

        [HttpGet("genres/{genreId}/movies/{movieId}/reviews")]
        [Authorize]
        public async Task<IActionResult> GetReviewsByGenreAndMovie(int genreId, int movieId)
        {
            if (genreId <= 0 || movieId <= 0)
            {
                return BadRequest("GenreId and MovieId must be greater than 0.");
            }

            var result = await _reviewService.GetReviewsByGenreAndMovie(genreId, movieId);

            if (!result.Any())
            {
                return NotFound($"No reviews found for genreId {genreId} and movieId {movieId}.");
            }

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviews()
        {
            var result = await _reviewService.GetAllAsync();
            return Ok(result);
        }
    }
}
