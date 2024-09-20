using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Category
{
    public class CreateCategoryRequestDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string Description { get; set; } = string.Empty;

        public List<int>? ItemIds { get; set; }


    }
}