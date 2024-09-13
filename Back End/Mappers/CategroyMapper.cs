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
         public static CategoryResponseDto CategoryResponseDto(this CategoryModel category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }

        public static CategoryModel ToCategoryModel(this CreateCategoryRequestDto categoryRequestDto)
        {
            return new CategoryModel
            {
                Name = categoryRequestDto.Name,
                Description = categoryRequestDto.Description
            };
        }
    }
}