using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.User
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(20, ErrorMessage =  "Username cannot be longer then 20 characters."), 
        MinLength(3, ErrorMessage = "Username cannot be shorter then 3 characters.")]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}