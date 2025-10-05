using Microsoft.AspNetCore.Mvc;
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

            var result = await _reviewService.CreateReviewAsync(request);
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

            var result = await _reviewService.UpdateReviewAsync(request);

            if (result == null)
            {
                return NotFound($"Review with id {request.Id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReview([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be equal or less than 0");
            }

            var deleted = await _reviewService.DeleteReviewAsync(id);

            if (!deleted)
            {
                return NotFound($"Review with id {id} not found.");
            }

            return NoContent();
        }
    }
}
