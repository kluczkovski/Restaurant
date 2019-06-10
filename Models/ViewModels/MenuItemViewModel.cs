using System;
using System.Collections.Generic;

namespace Restaurant.Models.ViewModels
{
    public class MenuItemViewModel
    {
        public MenuItem MenuItem { get; set; }
        public IEnumerable<Category>Categories { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
        public string StatusMessage { get; set; }

        public MenuItemViewModel()
        {
        }
    }
}
