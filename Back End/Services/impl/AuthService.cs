using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.User;
using Back_End.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Services.impl
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterResultDTO> RegisterUserAsync(RegisterDTO registerDTO)
        {
            var user = new UserModel
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return new RegisterResultDTO { Succeeded = false, Errors = result.Errors };
            }

            var token = await _tokenService.CreateTokenAsync(user);

            return new RegisterResultDTO
            {
                Succeeded = true,
                NewUser = new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token
                }
            };


        }

        public async Task<LoginResultDTO> LoginUserAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.Username);

            if (user == null)
            {
                return new LoginResultDTO
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials" }
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return new LoginResultDTO
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials" }
                };
            }

            var token = await _tokenService.CreateTokenAsync(user);

            return new LoginResultDTO
            {
                Success = true,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }
    }
}