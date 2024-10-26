using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data.Repositories;
using Back_End.DTOs;
using Back_End.DTOs.Category;
using Back_End.Mappers;
using Back_End.Models;

namespace Back_End.Services.impl
{
    public class CategoryService : ICategoryService
    {


        private readonly ICategoryRepository _categoryRepository;
        private readonly ICacheService _cache;

        private const string AllCategoriesCacheKey = "categories";

        public CategoryService(ICategoryRepository categoryRepository, ICacheService cache)
        {
            _categoryRepository = categoryRepository;
            _cache = cache;
        }


        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var cachedCategories = await _cache.GetAsync<List<CategoryResponseDto>>(AllCategoriesCacheKey);
            if (cachedCategories != null)
            {
                return cachedCategories;
            }

            var categories = await _categoryRepository.GetAllAsync();
            var categoryResponseDtos = categories.Select(c => c.ToCategoryResponseDto()).ToList();
            await _cache.SetAsync(AllCategoriesCacheKey, categoryResponseDtos, TimeSpan.FromMinutes(10));
            return categoryResponseDtos;
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found"); // Or handle this with custom exception handling
            }
            var categoryResponseDto = category.ToCategoryResponseDto();
            return categoryResponseDto;
        }

        public async Task<CategoryModel> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto)
        {
            var categoryModel = createCategoryRequestDto.ToCategoryModel();
            var createdCategory = await _categoryRepository.CreateAsync(categoryModel, createCategoryRequestDto.ItemIds);

            // Invalidate the all categories cache
            await _cache.ClearAsync(AllCategoriesCacheKey);

            return createdCategory;
        }

        public async Task<CategoryModel?> UpdateAsync(int id, UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory, updateCategoryRequestDto);

            // Invalidate caches
            await _cache.ClearAsync(AllCategoriesCacheKey);

            return updatedCategory;

        }

        public async Task<CategoryModel?> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            var deletedCategory = await _categoryRepository.DeleteAsync(category);

            // Invalidate caches
            await _cache.ClearAsync(AllCategoriesCacheKey);

            return deletedCategory;
        }
    }
}