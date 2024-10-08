using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Item;
using Back_End.Models;

namespace Back_End.Mappers
{
    public static class ItemMapper
    {
        

        public static ItemResponseDto ToItemResponseDto(this ItemModel item)
        {
            return new ItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Publisher = item.Publisher,
                IsPaid = item.IsPaid,
                Image = item.Image,
                Reviews = item.Reviews.Select(review => review.ToReviewResponseDto()).ToList(),
                CategoryIds = item.Categories.Select(c => c.Id).ToList(),
                CategoryNames = item.Categories.Select(c => c.Name).ToList()
            };
        }

        public static ItemModel ToItemModel(this CreateItemRequestDto createItemRequestDto)
        {
            return new ItemModel
            {
                Name = createItemRequestDto.Name,
                Description = createItemRequestDto.Description,
                Publisher = createItemRequestDto.Publisher,
                IsPaid = createItemRequestDto.IsPaid,
                Image = createItemRequestDto.Image,
                Categories = createItemRequestDto.CategoryIds.Select(id => new CategoryModel { Id = id }).ToList()
            };
        }

           public static ItemModel ToItemFromUpdate(this UpdateItemRequestDto updateItemRequestDto)
        {
            return new ItemModel
            {
                Name = updateItemRequestDto.Name,
                Description = updateItemRequestDto.Description,
                Publisher = updateItemRequestDto.Publisher,
                IsPaid = updateItemRequestDto.IsPaid,
                Image = updateItemRequestDto.Image,
                Categories = updateItemRequestDto.CategoryIds.Select(id => new CategoryModel { Id = id }).ToList()
            };
        }

    }
}