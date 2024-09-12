using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data.Repositories;
using Back_End.DTOs;
using Back_End.Mappers;
using Back_End.Models;

namespace Back_End.Services.impl
{
    public class ItemService : IItemService
    {

        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemModel>> GetAllAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<ItemModel?> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new Exception("Item not found"); // Or handle this with custom exception handling
            }
            return item;
        }

        public async Task<ItemModel> CreateAsync(CreateItemRequestDto createItemRequestDto)
        {
            var stockModel = createItemRequestDto.ToItemModel();

            return await _itemRepository.CreateAsync(stockModel);
        }

        public async Task<ItemModel?> UpdateAsync(int id, UpdateItemRequestDto updateItemRequestDto)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);

            if (existingItem == null)
            {
                throw new Exception("Item not found");
            }
            return await _itemRepository.UpdateAsync(existingItem, updateItemRequestDto);

        }

        public async Task<ItemModel?> DeleteAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            return await _itemRepository.DeleteAsync(item);
        }
    }
}