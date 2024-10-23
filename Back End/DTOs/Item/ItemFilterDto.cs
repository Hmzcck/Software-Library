using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
    public class ItemFilterDto
    {
        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }
        public List<int>? CategoryIds { get; set; }

        [MaxLength(100, ErrorMessage = "Publisher cannot be longer than 100 characters.")]
        public string? Publisher { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? MinRating { get; set; }

        public Boolean MostStars { get; set; } = false;
    
        public Boolean MostForks { get; set; } = false;

        public Boolean MostRecent { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]

        public int PageSize { get; set; } = 15;
    }

}