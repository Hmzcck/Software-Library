using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Review;
using Back_End.Models;

namespace Back_End.Data.Repositories
{
    public interface IUserFavoriteItemRepository
    {
        Task<List<UserFavoriteItem>> GetUserFavoriteItems(string userId);
        Task<UserFavoriteItem?> GetUserFavoriteItem(string userId, int itemId);
        Task<UserFavoriteItem> AddUserFavoriteItem(UserFavoriteItem userFavoriteItem);
        Task<UserFavoriteItem?> RemoveUserFavoriteItem(UserFavoriteItem userFavoriteItem);
    }
}