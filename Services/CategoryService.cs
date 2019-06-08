using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using Restaurant.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

using Restaurant.Services.Exceptions;

namespace Restaurant.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Find by Id
        public async Task<Category> FindByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new NotFoundException("Id not found!!!");
            }

            Category category =  await _db.Category.FindAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Not found Category for id: " + id);
            }
            return category;
        }


        // Find All by Async
        public async Task<List<Category>> FindAllAsync()
        {
            return await _db.Category.OrderBy(x => x.Name).ToListAsync();
        }


        // Insert a new record
        public async Task InsertAsync(Category category)
        {
            _db.Add(category);
            await _db.SaveChangesAsync();
        }


        // Update a record
        public async Task UpdateAsync(Category category)
        {

            bool hasAny = await _db.Category.AnyAsync(x => x.Id == category.Id);
            if (! hasAny )
            {
                throw new NotFoundException("Id not found!!!");
            }

            try
            {
                _db.Update(category);
                await _db.SaveChangesAsync();
            }
            catch (DbConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }

        }

        // Delete a record
        public async Task DeleteAsync(Category category)
        {

            bool hasAny = await _db.Category.AnyAsync(x => x.Id == category.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found!!!");
            }

            try
            {
                _db.Remove(category);
                await _db.SaveChangesAsync();
            }
            catch (DbConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }

        }
    }
}
