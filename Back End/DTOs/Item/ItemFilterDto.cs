using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
    public class ItemFilterDto
    {
        public string? Name { get; set; }
        public List<int>? CategoryIds { get; set; }
        public string? Publisher { get; set; }
        public int? MinRating { get; set; }
    }

}