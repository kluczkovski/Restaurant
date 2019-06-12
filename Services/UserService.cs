using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Restaurant.Data;
using System.Threading.Tasks;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }


        //Get all User
        public async Task<IEnumerable<ApplicationUser>> GetAllUser()
        {
            return await _db.ApplicationUser
                         .OrderBy(u => u.Name)
                         .ToListAsync();
        }


        //Get all User excluiding one (string
        public async Task<IEnumerable<ApplicationUser>> GetAllExUser(string id)
        {
            return await _db.ApplicationUser
                        .Where(u => u.Id != id)
                        .OrderBy(u => u.Name).ToListAsync();
        }


        //Get by Id
        public async Task<ApplicationUser> GetById(string id)
        {
            return await _db.ApplicationUser.FirstOrDefaultAsync(user => user.Id == id);
        }


        //Set UserLock
        public void SetLock(ApplicationUser user)
        {
            user.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        //Set UserLock
        public void SetUnLock(ApplicationUser user)
        {
            user.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }

    }
}
