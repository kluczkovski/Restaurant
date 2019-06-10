using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using Restaurant.Data;
using Restaurant.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Restaurant.Services
{
    public class MenuItemService
    {
        private readonly ApplicationDbContext _db;
        public string StatusMessage { get; set; }


        public MenuItemService(ApplicationDbContext db)
        {
            _db = db;
        }


        // Find MenuItem by Id
        public async Task<MenuItem> FindByIdAsync(int id)
        {
            return await _db.MenuItem
                .Include(obj => obj.Category)
                .Include(obj => obj.SubCategory)
                .FirstOrDefaultAsync(obj => obj.Id == id);
        }


        //Find all MenuItems
        public async Task<IEnumerable<MenuItem>> FindAllAsync()
        {
            return await _db.MenuItem.OrderBy(obj => obj.Name).ToListAsync();
        }


        //Create a new record
        public async Task InsertAsync(MenuItem menuItem)
        {
            if (Validation(menuItem))
            {
                await _db.AddAsync(menuItem);
                _db.SaveChanges();
            }
        }


        //Update a Record
        public async Task UpdateAsync(MenuItem menuItem)
        {

            if (Validation(menuItem))
            {
                _db.Update(menuItem);
                StatusMessage = $"Success : SubCategory {menuItem.Name} was updated";
                await _db.SaveChangesAsync();
            }
        }


        //Delete a Record
        public async Task DeleteAsync(MenuItem menuItem)
        {
            _db.MenuItem.Remove(menuItem);
            await _db.SaveChangesAsync();
        }


        //Get Image Field
        public async Task<string> GetImageField(int id)
        {
            return await _db.MenuItem
                        .Where(mi => mi.Id == id)
                        .Select(mi => mi.Image).FirstAsync();
        }

       
        //Validations
        public bool Validation(MenuItem subCategory)
        {
            return true;
        }
    }
}
