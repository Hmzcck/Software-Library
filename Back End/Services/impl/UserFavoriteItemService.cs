using System.Security.Claims;
using api.Extensions;
using Back_End.Data.Repositories;
using Back_End.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Services.impl
{
    public class UserFavoriteItemService : IUserFavoriteItemService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IItemRepository _itemRepository;
        private readonly IUserFavoriteItemRepository _userFavoriteItemRepository;

        public UserFavoriteItemService(UserManager<UserModel> userManager, IItemRepository itemRepository, IUserFavoriteItemRepository userFavoriteItemRepository)
        {
            _userManager = userManager;
            _itemRepository = itemRepository;
            _userFavoriteItemRepository = userFavoriteItemRepository;
        }

        public async Task<List<UserFavoriteItem>> GetUserFavoriteItems(ClaimsPrincipal User)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            return await _userFavoriteItemRepository.GetUserFavoriteItems(user.Id);
        }


        public async Task<UserFavoriteItem> AddUserFavoriteItem(ClaimsPrincipal User, int ItemId)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            var item = await _itemRepository.GetByIdAsync(ItemId);

            if (item == null)
            {
                throw new Exception("Item not found");
            }

            var existingUserFavoriteItem = await _userFavoriteItemRepository.GetUserFavoriteItem(user.Id, ItemId);

            if (existingUserFavoriteItem != null)
            {
                throw new Exception("Item already in favorites");
            }

            var userFavoriteItem = new UserFavoriteItem
            {
                UserId = user.Id,
                ItemId = ItemId
            };

            return await _userFavoriteItemRepository.AddUserFavoriteItem(userFavoriteItem);

        }

        public async Task<UserFavoriteItem> RemoveUserFavoriteItem(ClaimsPrincipal User, int ItemId)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            var existingUserFavoriteItem = await _userFavoriteItemRepository.GetUserFavoriteItem(user.Id, ItemId);

            if (existingUserFavoriteItem == null)
            {
                throw new Exception("Item not in favorites");
            }

            return await _userFavoriteItemRepository.RemoveUserFavoriteItem(existingUserFavoriteItem);
        }
    }
}