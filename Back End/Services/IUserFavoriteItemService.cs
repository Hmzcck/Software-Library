using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Item;
using Back_End.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Services
{
    public interface IUserFavoriteItemService
    {
        public Task<PaginatedResponse<ItemResponseDto>> GetUserFavoriteItems(ClaimsPrincipal User, ItemFilterDto itemFilterDto);

        public Task<UserFavoriteItem> AddUserFavoriteItem(ClaimsPrincipal User, int ItemId);

        public Task<UserFavoriteItem> RemoveUserFavoriteItem(ClaimsPrincipal User, int ItemId);

    }
}