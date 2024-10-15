using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data;
using Back_End.Data.Repositories;
using Back_End.DTOs.Item;
using Back_End.Mappers;
using Back_End.Models;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ItemFilterDto itemFilterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var items = await _itemService.GetAllAsync(itemFilterDto);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var item = await _itemService.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]  CreateItemRequestDto createItemRequestDto)//itemdto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ItemModel item = await _itemService.CreateAsync(createItemRequestDto);
            
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item.ToItemResponseDto());
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemRequestDto updateItemRequestDto) //itemdto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _itemService.UpdateAsync(id, updateItemRequestDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _itemService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("add-category")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequestDto addCategoryRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _itemService.AddCategoryAsync(addCategoryRequestDto);
            return NoContent();
        }   
    }
}