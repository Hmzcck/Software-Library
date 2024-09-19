using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.Models
{
    [Table("Items")]
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public String Publisher { get; set; } = string.Empty;

        public Boolean IsPaid { get; set; } 

        public String Image { get; set; } = string.Empty;

        public List<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();

        public ICollection<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

       public List<UserFavoriteItem> UserFavoriteItems { get; set; } = new List<UserFavoriteItem>();
    }
}