using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;

namespace Back_End.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? ItemId { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }

}