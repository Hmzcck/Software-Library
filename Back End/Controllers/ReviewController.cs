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
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAllAsync();
            return Ok(reviews);
        }       

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]  CreateReviewRequestDto createreviewRequestDto)
        {
            ReviewModel review = await _reviewService.CreateAsync(createreviewRequestDto);
            
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review.ToReviewResponseDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReviewRequestDto updatereviewRequestDto) 
        {
            await _reviewService.UpdateAsync(id, updatereviewRequestDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _reviewService.DeleteAsync(id);
            return NoContent();
        }
    }
}