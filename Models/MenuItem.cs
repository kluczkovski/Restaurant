﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Restaurant.Models.Enums;

namespace Restaurant.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ESpicy Spicyness { get; set; }

        public string Image { get; set; }

        [Display(Name = "Category")]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }


        [Display(Name = "SubCategory")]
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        public int SubCategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Price should be greater than ${1}")]
        public double Price { get; set; }


        public MenuItem()
        {
        }
    }
}
