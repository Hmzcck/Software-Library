using System.Security.Claims;
using api.Extensions;
using Back_End.Data.Repositories;
using Back_End.DTOs.Item;
using Back_End.Mappers;
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

        public async Task<PaginatedResponse<ItemResponseDto>> GetUserFavoriteItems(ClaimsPrincipal User, ItemFilterDto itemFilterDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);


            var paginatedResponse = await _userFavoriteItemRepository.GetUserFavoriteItems(user.Id, itemFilterDto);
            return paginatedResponse;
        }


        public async Task<UserFavoriteItem> AddUserFavoriteItem(ClaimsPrincipal User, int ItemId)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            var item = await _itemRepository.GetByIdAsync(ItemId);

            if (item == null)
            {
                throw new KeyNotFoundException("item was not found");
            }

            var existingUserFavoriteItem = await _userFavoriteItemRepository.GetUserFavoriteItem(user.Id, ItemId);

            if (existingUserFavoriteItem != null)
            {
                throw new ArgumentException("items is already in favorites");
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
                throw new KeyNotFoundException("items was not found in favorites");
            }

            return await _userFavoriteItemRepository.RemoveUserFavoriteItem(existingUserFavoriteItem);
        }
    }
}