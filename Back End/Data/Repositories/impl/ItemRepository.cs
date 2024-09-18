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

        public async Task<List<ItemModel>> GetAllAsync()
        {
            return await _context.Items.Include(Item => Item.Reviews)
            .Include(Item => Item.Categories)
            .ToListAsync();
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
            return await _context.Items.Include(Item => Item.Reviews)
            .Include(Item => Item.Categories.Select(Category => Category.Id))
            .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ItemModel?> UpdateAsync(ItemModel existingItem, UpdateItemRequestDto updateItemRequestDto)
        {
            existingItem.Name = updateItemRequestDto.Name;
            existingItem.Description = updateItemRequestDto.Description;
            existingItem.Publisher = updateItemRequestDto.Publisher;
            existingItem.IsPaid = updateItemRequestDto.IsPaid;
            existingItem.Image = updateItemRequestDto.Image;
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