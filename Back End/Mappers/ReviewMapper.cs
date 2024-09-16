using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Review;
using Back_End.Models;

namespace Back_End.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewResponseDto ToReviewResponseDto(this ReviewModel review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                Name = review.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public static ReviewModel ToReviewModel(this CreateReviewRequestDto reviewRequestDto)
        {
            return new ReviewModel
            {
                Name = reviewRequestDto.Name,
                Rating = reviewRequestDto.Rating,
                Comment = reviewRequestDto.Comment,
            };
        }
    }
}