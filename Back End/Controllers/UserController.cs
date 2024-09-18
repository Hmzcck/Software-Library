using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.User;
using Back_End.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {   
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)

        {
            _authService = authService;    
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var result = await _authService.RegisterUserAsync(registerDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.NewUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _authService.LoginUserAsync(loginDto);

            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}