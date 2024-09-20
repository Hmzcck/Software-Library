using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data;
using Back_End.Data.Repositories;
using Back_End.DTOs.Review;
using Back_End.Mappers;
using Back_End.Models;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviews = await _reviewService.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        [Authorize]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await _reviewService.GetByIdAsync(id);
            return Ok(review);
        }

        [HttpPost]
        [Route("{itemId:int}")]
        [Authorize]

        public async Task<IActionResult> Create([FromRoute] int itemId, CreateReviewRequestDto createReviewRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewModel review = await _reviewService.CreateAsync(itemId, createReviewRequestDto);

            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review.ToReviewResponseDto());
        }

        [HttpPut("{id:int}")]
        [Authorize]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReviewRequestDto updateReviewRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _reviewService.UpdateAsync(id, updateReviewRequestDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _reviewService.DeleteAsync(id);
            return NoContent();
        }
    }
}