using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Back_End.DTOs.User
{
    public class RegisterResultDTO
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
        public NewUserDTO NewUser { get; set; }
    }
}