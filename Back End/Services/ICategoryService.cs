using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.Category;
using Back_End.Models;

namespace Back_End.Services
{
    public interface ICategoryService
    {
        //CRUD

        public  Task<List<CategoryModel>> GetAllAsync();

        public Task<CategoryModel?> GetByIdAsync(int id);

        public Task<CategoryModel> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto);

        public Task<CategoryModel?> UpdateAsync(int id, UpdateCategoryRequestDto updateCategoryRequestDto);

        public Task<CategoryModel?> DeleteAsync(int id);
    }
}