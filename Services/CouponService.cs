using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


using Restaurant.Data;
using Restaurant.Models;
using System.IO;

namespace Restaurant.Services
{
    public class CouponService
    {
        private readonly ApplicationDbContext _db;
        public string StatusMessage { get; set; }


        public CouponService(ApplicationDbContext db)
        {
            _db = db;
        }


        //Get By ID
        public async Task<Coupon> GetByIdAsync(int id)
        {
            return await _db.Coupon.FindAsync(id);
        }


        //Get All, order by typ of Coupon
        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            return await _db.Coupon
                        .OrderBy(c =>  c.Name)
                        .OrderBy(c => c.CouponType).ToListAsync();
        }


        //Get All that are active
        public async Task<IEnumerable<Coupon>> GetAllCouponsIsActiveAsync()
        {
            return await _db.Coupon
                        .Where(c => c.IsActive == true)
                        .OrderBy(c => c.Name).ToListAsync();
        }


        //Create a new record
        public async Task InsertAsync(Coupon coupon)
        {
            if (Validation(coupon))
            {
                await _db.AddAsync(coupon);
                _db.SaveChanges();
            }
        }


        //Update a Record
        public async Task UpdateAsync(Coupon coupon)
        {

            if (Validation(coupon))
            {
                _db.Update(coupon);
                StatusMessage = $"Success : SubCategory {coupon.Name} was updated";
                await _db.SaveChangesAsync();
            }
        }


        //Delete a Record
        public async Task DeleteAsync(Coupon coupon)
        {
            _db.Coupon.Remove(coupon);
            await _db.SaveChangesAsync();
        }


        //Update Picture
        public bool UpdatePicture(Coupon coupon, Stream file)
        {
            byte[] aux = null;

            using (var ms = new MemoryStream())
                {
                file.CopyTo(ms);
                    aux = ms.ToArray();
                }

            coupon.Picture = aux;
            return true;
        }


        //Validations
        public bool Validation(Coupon coupon)
        {
            return true;
        }


    }
}
