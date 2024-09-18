using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.DTOs.Item
{
  public class AddCategoryRequestDto
  {
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
  }
}