using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Item;
using Back_End.Models;

namespace Back_End.Data.Repositories
{
    public interface IItemRepository
    {
        Task<PaginatedResponse<ItemResponseDto>> GetAllAsync(ItemFilterDto itemFilterDto);
        Task<ItemModel?> GetByIdAsync(int id);
        Task<ItemModel> CreateAsync(ItemModel itemModel);
        Task<ItemModel?> UpdateAsync(ItemModel existingItem, UpdateItemRequestDto updateItemRequestDto);
        Task<ItemModel?> DeleteAsync(ItemModel item);

        Task<ItemModel?> AddCategoryAsync(ItemModel item, CategoryModel category);


    }
}