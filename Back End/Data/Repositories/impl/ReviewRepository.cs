using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Review;
using Back_End.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data.Repositories.impl
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDBContext _context;

        public ReviewRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewModel>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }


        public async Task<ReviewModel?> DeleteAsync(ReviewModel review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<ReviewModel> CreateAsync(ReviewModel reviewModel)
        {
            // Add the Review to the context
            _context.Reviews.Add(reviewModel);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created Review
            return reviewModel;
        }


        public async Task<ReviewModel?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<ReviewModel?> UpdateAsync(ReviewModel existingReview, UpdateReviewRequestDto updateReviewRequestDto)
        {
            existingReview.Name = updateReviewRequestDto.Name;
            existingReview.Rating = updateReviewRequestDto.Rating;
            existingReview.Comment = updateReviewRequestDto.Comment;

            await _context.SaveChangesAsync();

            return existingReview;
        }
    }
}