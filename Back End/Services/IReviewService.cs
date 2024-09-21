using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Review;
using Back_End.Models;

namespace Back_End.Services
{
    public interface IReviewService
    {
        //CRUD

        public  Task<List<ReviewResponseDto>> GetAllAsync();

        public Task<ReviewModel?> GetByIdAsync(int id);

        public Task<ReviewModel> CreateAsync(int itemId,ClaimsPrincipal User ,CreateReviewRequestDto createReviewRequestDto);

        public Task<ReviewModel?> UpdateAsync(int id, UpdateReviewRequestDto updateReviewRequestDto);

        public Task<ReviewModel?> DeleteAsync(int id);
    }
}