using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Category;
using Back_End.Models;

namespace Back_End.Mappers
{
    public static class CategroyMapper
    {
         public static CategoryResponseDto ToCategoryResponseDto(this CategoryModel category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ItemIds = category.Items?.Select(i => i.Id).ToList()
            };
        }

        public static CategoryModel ToCategoryModel(this CreateCategoryRequestDto categoryRequestDto)
        {
            return new CategoryModel
            {
                Name = categoryRequestDto.Name,
                Description = categoryRequestDto.Description,
            };
        }
    }
}