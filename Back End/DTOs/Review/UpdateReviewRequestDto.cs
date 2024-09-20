using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Review
{
    public class UpdateReviewRequestDto
    {
        [MaxLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string Name { get; set; } = string.Empty;
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(300, ErrorMessage = "Comment cannot be longer than 300 characters.")]
        public string Comment { get; set; } = string.Empty;
    }
}