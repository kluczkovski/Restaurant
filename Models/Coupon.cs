using System;
using System.ComponentModel.DataAnnotations;

using Restaurant.Models.Enums;

namespace Restaurant.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ECoupon CouponType { get; set; }

        [Required]
        public double Discount { get; set; }

        [Required]
        public double MinimumAmount { get; set; }

        public byte[] Picture { get; set; }

        public bool IsActive { get; set; }

        public Coupon()
        {
        }


    }
}
