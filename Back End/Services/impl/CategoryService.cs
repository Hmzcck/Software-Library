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

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<CategoryModel?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found"); // Or handle this with custom exception handling
            }
            return category;
        }

        public async Task<CategoryModel> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto)
        {
            var categoryModel = createCategoryRequestDto.ToCategoryModel();

            return await _categoryRepository.CreateAsync(categoryModel);
        }

        public async Task<CategoryModel?> UpdateAsync(int id, UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }
            return await _categoryRepository.UpdateAsync(existingCategory, updateCategoryRequestDto);

        }

        public async Task<CategoryModel?> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            return await _categoryRepository.DeleteAsync(category);
        }
    }
}