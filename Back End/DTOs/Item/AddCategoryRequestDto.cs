using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
  public class AddCategoryRequestDto
  {
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int CategoryId { get; set; }
  }
}