using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Restaurant.Data;
using Restaurant.Models;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Services
{
    public class SubCategoryService
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public SubCategoryService(ApplicationDbContext db)
        {
            _db = db;
        }


        //Get SubCategory by Id
        public async Task<SubCategory> FindByIdSync(int id)
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


        //Get All SubCategories
        public IEnumerable<SubCategory> FindAll()
        {
            return  _db.SubCategory
                        .Include(c => c.Category)
                        .OrderBy(c => c.Category.Name)
                        .OrderBy(s => s.Name)
                        .ToList();
        }

        //Get All Name SubCategory
        public async Task<List<string>> FindAllNameSCAsync()
        {
            return await _db.SubCategory
                        .Include(c => c.Category)
                        .OrderBy(c => c.Category.Name)
                        .OrderBy(s => s.Name)
                        .Select(s => s.Name)
                        .ToListAsync();
        }


        //Create a new Record
        public async Task InsertAsync(SubCategory subCategory)
        {
            if (Validation(subCategory))
            {
                StatusMessage = $"Success : SubCategory {subCategory.Name} was created";
                await _db.AddAsync(subCategory);
                await _db.SaveChangesAsync();
            }
        }


        //Update a Record
        public async Task UpdateAsync(SubCategory subCategory)
        {
            if (Validation(subCategory))
            {
                _db.Update(subCategory);
                StatusMessage = $"Success : SubCategory {subCategory.Name} was updated";

                await _db.SaveChangesAsync();
            }
        }


        //Delete a Record
        public async Task DeleteAsync(SubCategory subCategory)
        {
            _db.SubCategory.Remove(subCategory);
           await _db.SaveChangesAsync();
        }


        public bool  Validation(SubCategory subCategory)
        {
            var numberSubC = _db.SubCategory
                .Where(s => s.Name == subCategory.Name && s.CategoryId==subCategory.CategoryId && s.Id!=subCategory.Id) 
                .Count();
            if (numberSubC > 0)
            {
                StatusMessage = "Error : SubCategory already exist! Use another name."; 
                return false;
            }
            return true;
        }


        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesByCategoryAsync(int id)
        {
            //return await _db.SubCategory.Where(sub => sub.CategoryId == id).ToListAsync();
            return await (from subcategory in _db.SubCategory
                         where subcategory.CategoryId == id
                         orderby subcategory.Name
                         select subcategory).ToListAsync();
        }


    }
}

