using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Extensions;
using Back_End.Data.Repositories;
using Back_End.DTOs;
using Back_End.DTOs.Review;
using Back_End.Mappers;
using Back_End.Models;
using Microsoft.AspNetCore.Identity;

namespace Back_End.Services.impl
{
    public class ReviewService : IReviewService
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IItemRepository _itemRepository;

        private readonly UserManager<UserModel> _userManager;

        public ReviewService(IReviewRepository reviewRepository, IItemRepository itemRepository, UserManager<UserModel> userManager)
        {
            _reviewRepository = reviewRepository;
            _itemRepository = itemRepository;
            _userManager = userManager;
        }

        public async Task<List<ReviewResponseDto>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Select(r => r.ToReviewResponseDto()).ToList();
        }

        public async Task<ReviewModel?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException("review was not found"); // Or handle this with custom exception handling
            }
            return review;
        }

        public async Task<ReviewModel> CreateAsync(int itemId, ClaimsPrincipal User, CreateReviewRequestDto createReviewRequestDto)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null)
            {
                throw new KeyNotFoundException("item was not found"); // Or handle this with custom exception handling
            }

            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            var reviewModel = createReviewRequestDto.ToReviewModel(item.Id);

            reviewModel.UserId = user.Id;

            return await _reviewRepository.CreateAsync(reviewModel);
        }

        public async Task<ReviewModel?> UpdateAsync(int id, UpdateReviewRequestDto updateReviewRequestDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);

            if (existingReview == null)
            {
                throw new KeyNotFoundException("review was not found");
            }
            return await _reviewRepository.UpdateAsync(existingReview, updateReviewRequestDto);

        }

        public async Task<ReviewModel?> DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException("review was not found");
            }

            return await _reviewRepository.DeleteAsync(review);
        }
    }
}