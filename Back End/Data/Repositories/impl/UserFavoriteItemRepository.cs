using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        public async Task<List<UserFavoriteItem>> GetUserFavoriteItems(string userId)
        {
            return await _context.UserFavoriteItems.Where(x => x.UserId == userId).ToListAsync();
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