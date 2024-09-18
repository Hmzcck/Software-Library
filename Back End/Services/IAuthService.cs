using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.User;
using Back_End.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Services
{
    public interface IAuthService
    {
        public  Task<RegisterResultDTO> RegisterUserAsync(RegisterDTO registerDTO);

        public Task<LoginResultDTO> LoginUserAsync(LoginDTO loginDTO);
    }
}