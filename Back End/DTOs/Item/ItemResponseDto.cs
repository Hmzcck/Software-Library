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
        public String Publisher { get; set; } = string.Empty;

        public Boolean IsPaid { get; set; }

        public String Image { get; set; } = string.Empty;

        public List<ReviewResponseDto> Reviews { get; set; }
    }

}