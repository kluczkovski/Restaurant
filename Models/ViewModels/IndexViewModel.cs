using System;
using System.Collections.Generic;

namespace Restaurant.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<MenuItem> MenuItem { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
        public IEnumerable<Coupon> Coupon { get; set; }


        public IndexViewModel()
        {

        }
    }
}
