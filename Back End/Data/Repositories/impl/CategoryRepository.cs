using Back_End.DTOs.Category;
using Back_End.Models;
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
            return await _context.Categories
                .Include(c => c.Items)
                .ToListAsync();
        }



        public async Task<CategoryModel?> DeleteAsync(CategoryModel category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel categoryModel, List<int> itemIds)
        {
            var items = await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToListAsync();
                
            categoryModel.Items = items;

            _context.Categories.Add(categoryModel);

            await _context.SaveChangesAsync();

            return categoryModel;
        }


        public async Task<CategoryModel?> GetByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.Items).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<CategoryModel?> UpdateAsync(CategoryModel existingcategory, UpdateCategoryRequestDto updatecategoryRequestDto)
        {
            existingcategory.Name = updatecategoryRequestDto.Name;
            existingcategory.Description = updatecategoryRequestDto.Description;

            await _context.SaveChangesAsync();

            return existingcategory;
        }

        public async Task<List<CategoryModel?>> GetCategoriesByIdsAsync(List<int> categoryIds)
        {
            return await _context.Categories
                                 .Where(c => categoryIds.Contains(c.Id))
                                 .ToListAsync();
        }
    }
}