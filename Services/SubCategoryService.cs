using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Restaurant.Data;
using Restaurant.Models;


namespace Restaurant.Services
{
    public class SubCategoryService
    {
        private readonly ApplicationDbContext _db;

        public SubCategoryService(ApplicationDbContext db)
        {
            _db = db;
        }


        //Get SubCategory by Id
        public async Task<SubCategory> GetByIdSync(int id)
        {
            return await _db.SubCategory
                   .Include(obj => obj.Category)
                   .FirstOrDefaultAsync(obj => obj.Id == id);
                   
        }

        //Get All SubCategories
        public async Task<IEnumerable<SubCategory>> FindAllAsync()
        {
            return await _db.SubCategory
                        .Include(c => c.Category)
                        .OrderBy(c => c.Category.Name)
                        .OrderBy(s => s.Name)
                        .ToListAsync();
        }
    }
}
