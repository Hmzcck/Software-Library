using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data.Repositories;
using Back_End.DTOs.Item;
using Back_End.Mappers;
using Back_End.Models;
using Newtonsoft.Json;

namespace Back_End.Services.impl
{
    public class ItemService : IItemService
    {

        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly ICacheService _cache;

        public ItemService(IItemRepository itemRepository, ICategoryRepository categoryRepository, ICacheService cache)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _cache = cache;
        }

        public async Task<List<ItemResponseDto>> GetAllAsync(ItemFilterDto itemFilterDto)
        {
            // caching
            string cacheKey = $"items_{JsonConvert.SerializeObject(itemFilterDto)}";

            var cachedItems = await _cache.GetAsync<List<ItemResponseDto>>(cacheKey);


            if (cachedItems != null)
            {
                return cachedItems;
            }


            var items = await _itemRepository.GetAllAsync(itemFilterDto);
            var itemResponseDtos = items.Select(ItemMapper.ToItemResponseDto).ToList();

            await _cache.SetAsync(cacheKey, itemResponseDtos, TimeSpan.FromMinutes(10));

            return itemResponseDtos;
        }

        public async Task<ItemResponseDto?> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException("item was not found"); // Or handle this with custom exception handling
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
                Stars = createItemRequestDto.Stars,
                Forks = createItemRequestDto.Forks,
                Repository = createItemRequestDto.Repository,
                Image = createItemRequestDto.Image,
                CreationDate = DateTime.Now,
                Categories = categories
            };

            var createdItem = await _itemRepository.CreateAsync(item);

            // Clear cache
            await _cache.ClearAsync("items_");

            return createdItem;
        }

        public async Task<ItemModel?> UpdateAsync(int id, UpdateItemRequestDto updateItemRequestDto)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);

            if (existingItem == null)
            {
                throw new KeyNotFoundException("item was not found");
            }
            var item = await _itemRepository.UpdateAsync(existingItem, updateItemRequestDto);

            await _cache.ClearAsync("items_");

            return item;

        }

        public async Task<ItemModel?> DeleteAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException("item was not found");
            }

            var result = await _itemRepository.DeleteAsync(item);

            await _cache.ClearAsync("items_");

            return result;
        }

        public async Task<ItemModel> AddCategoryAsync(AddCategoryRequestDto addCategoryRequestDto)
        {
            var item = await _itemRepository.GetByIdAsync(addCategoryRequestDto.ItemId);
            if (item == null)
            {
                throw new KeyNotFoundException("item was not found");
            }

            var category = await _categoryRepository.GetByIdAsync(addCategoryRequestDto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("category was not found");
            }
            await _itemRepository.AddCategoryAsync(item, category);

            await _cache.ClearAsync("items_");

            return item;
        }
    }
}