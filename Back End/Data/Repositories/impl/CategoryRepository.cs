using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Category;
using Back_End.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data.Repositories.impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;

        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }


        public async Task<CategoryModel?> DeleteAsync(CategoryModel category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel CategoryModel)
        {
            // Add the category to the context
            _context.Categories.Add(CategoryModel);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created category
            return CategoryModel;
        }


        public async Task<CategoryModel?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<CategoryModel?> UpdateAsync(CategoryModel existingcategory, UpdateCategoryRequestDto updatecategoryRequestDto)
        {
            existingcategory.Name = updatecategoryRequestDto.Name;
            existingcategory.Description = updatecategoryRequestDto.Description;

            await _context.SaveChangesAsync();

            return existingcategory;
        }
    }
}