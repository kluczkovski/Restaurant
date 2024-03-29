﻿using System;
using System.Collections.Generic;

namespace Restaurant.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<string> SubCategoryList { get; set; }
        public string StatusMessage { get; set; }

        public SubCategoryAndCategoryViewModel()
        {
        }
    }
}
