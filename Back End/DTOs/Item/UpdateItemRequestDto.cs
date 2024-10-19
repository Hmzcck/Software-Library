using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
    public class UpdateItemRequestDto
    {
        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; } = string.Empty;
        [MaxLength(100, ErrorMessage = "Publisher cannot be longer than 100 characters.")]
        public String Publisher { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue, ErrorMessage = "Stars must be greater than or equal to 0.")]
        public int Stars { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Forks must be greater than or equal to 0.")]
        public int Forks { get; set; }

        [MaxLength(300, ErrorMessage = "Image URL cannot be longer than 300 characters.")]
        public String Image { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public List<int> CategoryIds { get; set; } = new List<int>();

    }
}