using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Review
{
    public class CreateReviewRequestDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be longer than 30 characters."),
        MinLength(3, ErrorMessage = "Name cannot be shorter than 3 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [Required]
        [MaxLength(300, ErrorMessage = "Comment cannot be longer than 300 characters."),
        MinLength(10, ErrorMessage = "Comment cannot be shorter than 10 characters.")]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}