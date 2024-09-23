using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data.Repositories;
using Back_End.DTOs.Item;
using Back_End.Mappers;
using Back_End.Models;

namespace Back_End.Services.impl
{
    public class ItemService : IItemService
    {

        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ItemService(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ItemResponseDto>> GetAllAsync(ItemFilterDto itemFilterDto)
        {
            var items = await _itemRepository.GetAllAsync(itemFilterDto);
            return items.Select(ItemMapper.ToItemResponseDto).ToList();
        }

        public async Task<ItemResponseDto?> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new Exception("Item not found"); // Or handle this with custom exception handling
            }
            return ItemMapper.ToItemResponseDto(item);
        }

        public async Task<ItemModel> CreateAsync(CreateItemRequestDto createItemRequestDto)
        {
            var categories = await _categoryRepository.GetCategoriesByIdsAsync(createItemRequestDto.CategoryIds);

            var item = new ItemModel
            {
                Name = createItemRequestDto.Name,
                Description = createItemRequestDto.Description,
                Publisher = createItemRequestDto.Publisher,
                IsPaid = createItemRequestDto.IsPaid,
                Image = createItemRequestDto.Image,
                Categories = categories
            };
            return await _itemRepository.CreateAsync(item);
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

        public async Task<ItemModel> AddCategoryAsync(AddCategoryRequestDto addCategoryRequestDto)
        {
            var item = await _itemRepository.GetByIdAsync(addCategoryRequestDto.ItemId);
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            var category = await _categoryRepository.GetByIdAsync(addCategoryRequestDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            await _itemRepository.AddCategoryAsync(item, category);

            return item;
        }
    }
}