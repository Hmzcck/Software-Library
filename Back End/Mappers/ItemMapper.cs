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
        

        public static ItemResponseDto ItemResponseDto(this ItemModel item)
        {
            return new ItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Publisher = item.Publisher,
                IsPaid = item.IsPaid,
                Image = item.Image 
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
                Image = createItemRequestDto.Image
            };
        }

    }
}