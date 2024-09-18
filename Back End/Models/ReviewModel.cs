using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.Models
{
    [Table("Reviews")]
    public class ReviewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? ItemId { get; set; }

        public ItemModel? Item { get; set; } 

    }
}