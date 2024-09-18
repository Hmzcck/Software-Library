using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs;
using Back_End.DTOs.User;
using Back_End.Models;

namespace Back_End.Services
{
    public interface ITokenService
    {
        public  Task<string> CreateTokenAsync(UserModel user);

    }
}