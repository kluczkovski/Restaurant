using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }

        public MenuItem MenuItem { get; set; }
        public int MenuItemId { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {1}")]
        public int Count { get; set; }


        public ShoppingCart()
        {
            Count = 1;
        }
    }
}
