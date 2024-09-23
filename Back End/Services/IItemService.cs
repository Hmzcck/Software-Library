using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Item;
using Back_End.Models;

namespace Back_End.Services
{
    public interface IItemService
    {
        //CRUD

        public  Task<List<ItemResponseDto>> GetAllAsync(ItemFilterDto itemFilterDto);

        public Task<ItemResponseDto?> GetByIdAsync(int id);

        public Task<ItemModel> CreateAsync(CreateItemRequestDto createItemRequestDto);

        public Task<ItemModel?> UpdateAsync(int id, UpdateItemRequestDto updateItemRequestDto);

        public Task<ItemModel?> DeleteAsync(int id);

        public Task<ItemModel> AddCategoryAsync(AddCategoryRequestDto addCategoryRequestDto);
    }
}