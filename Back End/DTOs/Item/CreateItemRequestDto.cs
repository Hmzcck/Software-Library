using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
    public class CreateItemRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be longer then 100 characters."),
        MinLength(3, ErrorMessage = "Name cannot be shorter then 3 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(300, ErrorMessage = "Description cannot be longer then 300 characters."),
        MinLength(10, ErrorMessage = "Description cannot be shorter then 10 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Publisher cannot be longer then 100 characters.")]
        public String Publisher { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stars number must be greater than 0.")]

        public int Stars { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Forks number must be greater than 0.")]

        public int Forks { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage = "Repository URL cannot be longer than 300 characters.")]

        public string Repository { get; set; } = string.Empty;

        [Required]
        [MaxLength(300, ErrorMessage = "Image URL cannot be longer than 300 characters.")]

        public String Image { get; set; } = string.Empty;

        [Required]
        public List<int> CategoryIds { get; set; } = new List<int>();


    }
}