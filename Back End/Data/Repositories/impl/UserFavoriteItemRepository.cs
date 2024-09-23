using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Item;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data.Repositories.impl
{
    public class UserFavoriteItemRepository : IUserFavoriteItemRepository
    {

        private readonly ApplicationDBContext _context;

        public UserFavoriteItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<ItemModel>> GetUserFavoriteItems(string userId, ItemFilterDto itemFilterDto)
        {
            var query = _context.UserFavoriteItems
       .Where(x => x.UserId == userId)
       .Include(x => x.Item)               // Include the item first
           .ThenInclude(item => item.Reviews)
               .ThenInclude(review => review.User) // Include the User for each review
       .Include(x => x.Item)               // Include the item again
           .ThenInclude(item => item.Categories)  // Include Categories
       .Select(x => x.Item)                // Now you can project to select the Item
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

            return await query.ToListAsync();
        }

        public async Task<UserFavoriteItem?> GetUserFavoriteItem(string userId, int itemId)
        {
            return await _context.UserFavoriteItems.SingleOrDefaultAsync(x => x.UserId == userId && x.ItemId == itemId);
        }

        public async Task<UserFavoriteItem> AddUserFavoriteItem(UserFavoriteItem userFavoriteItem)
        {
            await _context.UserFavoriteItems.AddAsync(userFavoriteItem);
            await _context.SaveChangesAsync();
            return userFavoriteItem;
        }

        public async Task<UserFavoriteItem?> RemoveUserFavoriteItem(UserFavoriteItem userFavoriteItem)
        {
            _context.UserFavoriteItems.Remove(userFavoriteItem);
            await _context.SaveChangesAsync();
            return userFavoriteItem;
        }
    }
}