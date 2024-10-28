using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Item;
using Back_End.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data.Repositories.impl
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDBContext _context;

        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<ItemModel>> GetAllAsync(ItemFilterDto itemFilterDto)
        {
            var query = _context.Items
            .Include(item => item.Reviews)
                .ThenInclude(review => review.User)
            .Include(item => item.Categories)
            .AsQueryable();

            if (!string.IsNullOrEmpty(itemFilterDto.Name))
            {
                query = query.Where(item => item.Name.Contains(itemFilterDto.Name));
            }

            if (itemFilterDto.CategoryIds != null && itemFilterDto.CategoryIds.Any())
            {
                query = query.Where(item => item.Categories.Any(c => itemFilterDto.CategoryIds.Contains(c.Id)));
            }

            if (!string.IsNullOrEmpty(itemFilterDto.Publisher))
            {
                query = query.Where(item => item.Publisher.Contains(itemFilterDto.Publisher));
            }

            if (itemFilterDto.MinRating.HasValue)
            {
                query = query.Where(item => item.Reviews.Average(r => r.Rating) >= itemFilterDto.MinRating.Value);
            }

            if (itemFilterDto.MostStars)
            {
                query = query.OrderByDescending(item => item.Stars);
            }

            if (itemFilterDto.MostForks)
            {
                query = query.OrderByDescending(item => item.Forks);
            }

            if (itemFilterDto.MostRecent)
            {
                query = query.OrderByDescending(item => item.CreationDate);
            }

            return await query.Skip((itemFilterDto.PageNumber - 1) * itemFilterDto.PageSize)
    .Take(itemFilterDto.PageSize).ToListAsync();

        }


        public async Task<ItemModel?> DeleteAsync(ItemModel item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<ItemModel> CreateAsync(ItemModel itemModel)
        {
            // Add the item to the context
            _context.Items.Add(itemModel);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created item
            return itemModel;
        }


        public async Task<ItemModel?> GetByIdAsync(int id)
        {
            return await _context.Items
            .Include(item => item.Reviews)
              .ThenInclude(review => review.User)
            .Include(item => item.Categories)
            .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ItemModel?> UpdateAsync(ItemModel existingItem, UpdateItemRequestDto updateItemRequestDto)
        {
            existingItem.Name = updateItemRequestDto.Name;
            existingItem.Description = updateItemRequestDto.Description;
            existingItem.Publisher = updateItemRequestDto.Publisher;
            existingItem.Stars = updateItemRequestDto.Stars;
            existingItem.Forks = updateItemRequestDto.Forks;
            existingItem.Repository = updateItemRequestDto.Repository;
            existingItem.Image = updateItemRequestDto.Image;
            existingItem.CreationDate = updateItemRequestDto.CreationDate;
            existingItem.Categories = updateItemRequestDto.CategoryIds.Select(id => new CategoryModel { Id = id }).ToList();

            await _context.SaveChangesAsync();

            return existingItem;
        }

        public async Task<ItemModel?> AddCategoryAsync(ItemModel item, CategoryModel category)
        {

            if (!item.Categories.Contains(category))
            {
                item.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            return item;
        }
    }
}