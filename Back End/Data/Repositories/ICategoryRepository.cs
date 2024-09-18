using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Category;
using Back_End.Models;

namespace Back_End.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetAllAsync();
        Task<CategoryModel?> GetByIdAsync(int id);
        Task<CategoryModel> CreateAsync(CategoryModel categoryModel);
        Task<CategoryModel?> UpdateAsync(CategoryModel existingCategory, UpdateCategoryRequestDto updateCategoryRequestDto);
        Task<CategoryModel?> DeleteAsync(CategoryModel item);

        Task<List<CategoryModel?>> GetCategoriesByIdsAsync(List<int> categoryIds);
    }
}