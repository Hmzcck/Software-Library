using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.User
{
    public class LoginResultDTO
    {
        public bool Success { get; set; } 
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public string Token { get; set; } 
        public IEnumerable<string> Errors { get; set; } 
    }
}