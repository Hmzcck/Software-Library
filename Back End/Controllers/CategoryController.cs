using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data;
using Back_End.Data.Repositories;
using Back_End.DTOs.Category;
using Back_End.Mappers;
using Back_End.Models;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CategoryModel category = await _categoryService.CreateAsync(createCategoryRequestDto);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category.ToCategoryResponseDto());
        }

        [HttpPut("{id:int}")]
        [Authorize]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _categoryService.UpdateAsync(id, updateCategoryRequestDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}