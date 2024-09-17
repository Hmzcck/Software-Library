using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Review;
using Back_End.Models;

namespace Back_End.Data.Repositories
{
    public interface IReviewRepository
    {
        Task<List<ReviewModel>> GetAllAsync();
        Task<ReviewModel?> GetByIdAsync(int id);
        Task<ReviewModel> CreateAsync(ReviewModel reviewModel);
        Task<ReviewModel?> UpdateAsync(ReviewModel existingReview,UpdateReviewRequestDto updateReviewRequestDto);
        Task<ReviewModel?> DeleteAsync(ReviewModel item);
    }
}