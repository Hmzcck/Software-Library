using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data.Repositories;
using Back_End.DTOs;
using Back_End.DTOs.Review;
using Back_End.Mappers;
using Back_End.Models;

namespace Back_End.Services.impl
{
    public class ReviewService : IReviewService
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IItemRepository _itemRepository;

        public ReviewService(IReviewRepository reviewRepository, IItemRepository itemRepository)
        {
            _reviewRepository = reviewRepository;
            _itemRepository = itemRepository;
        }

        public async Task<List<ReviewModel>> GetAllAsync()
        {
            return await _reviewRepository.GetAllAsync();
        }

        public async Task<ReviewModel?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new Exception("Review not found"); // Or handle this with custom exception handling
            }
            return review;
        }

        public async Task<ReviewModel> CreateAsync(int itemId, CreateReviewRequestDto createReviewRequestDto)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null)
            {
                throw new Exception("Item not found"); // Or handle this with custom exception handling
            }
            var reviewModel = createReviewRequestDto.ToReviewModel(item.Id);

            return await _reviewRepository.CreateAsync(reviewModel);
        }

        public async Task<ReviewModel?> UpdateAsync(int id, UpdateReviewRequestDto updateReviewRequestDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);

            if (existingReview == null)
            {
                throw new Exception("Review not found");
            }
            return await _reviewRepository.UpdateAsync(existingReview, updateReviewRequestDto);

        }

        public async Task<ReviewModel?> DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new Exception("Review not found");
            }

            return await _reviewRepository.DeleteAsync(review);
        }
    }
}