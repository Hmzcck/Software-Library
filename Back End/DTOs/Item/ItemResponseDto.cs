using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.DTOs.Review;

namespace Back_End.DTOs.Item
{
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;

        public int Stars { get; set; }

        public int Forks { get; set; }

        public string Image { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public List<ReviewResponseDto> Reviews { get; set; }

        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<string> CategoryNames { get; set; } = new List<string>();
    }

}