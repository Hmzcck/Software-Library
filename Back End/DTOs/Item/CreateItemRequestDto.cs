using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
    public class CreateItemRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public String Publisher { get; set; } = string.Empty;

        public int Stars { get; set; }

        public int Forks { get; set; }

        public string Repository { get; set; } = string.Empty;

        public String Image { get; set; } = string.Empty;


        public List<int> CategoryIds { get; set; } = new List<int>();


    }
}