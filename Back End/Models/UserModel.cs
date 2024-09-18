using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Back_End.Models
{
    public class UserModel : IdentityUser
    {
      //  public List<UserFavoriteItem> FavoriteItems { get; set; } = new List<UserFavoriteItem>();
    }
}