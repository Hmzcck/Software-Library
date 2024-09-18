using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.Models
{
    [Table("UserFavoriteItems")]
    public class UserFavoriteItem
    {
        public string UserId { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public UserModel User { get; set; } = null!;
        public ItemModel Item { get; set; }  = null!;
    }
}