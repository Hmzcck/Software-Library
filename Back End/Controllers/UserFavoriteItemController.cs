using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using Back_End.DTOs.User;
using Back_End.Models;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [Route("api/userfavoriteitem")]
    [ApiController]
    public class UserFavoriteItemController : ControllerBase
    {   
        private readonly IUserFavoriteItemService _userFavoriteItemService;
        private readonly UserManager<UserModel> _userManager;

        public UserFavoriteItemController(IUserFavoriteItemService userFavoriteItemService, UserManager<UserModel> userManager)
        {
            _userFavoriteItemService = userFavoriteItemService;
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserFavoriteItems()
        {
            var userFavoriteItems =  await _userFavoriteItemService.GetUserFavoriteItems(User);
            return Ok(userFavoriteItems);
        }

        [HttpPost]
        [Route("{itemId:int}")]
        [Authorize]
        public async Task<IActionResult> AddUserFavoriteItem([FromRoute] int itemId)
        {
            try
            {
                var userFavoriteItem = await _userFavoriteItemService.AddUserFavoriteItem(User, itemId);
                return Ok(userFavoriteItem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{itemId:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveUserFavoriteItem([FromRoute] int itemId)
        {
            try
            {
                var userFavoriteItem = await _userFavoriteItemService.RemoveUserFavoriteItem(User, itemId);
                return Ok(userFavoriteItem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
