using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KolotreeWebApi.Models
{
    public class UserService
    {
        private readonly KolotreeDbContext db;

        public UserService(KolotreeDbContext _db)
        {
            db = _db;
        }


        public async Task<List<User>> FetchAllUsers()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<User> FindUser(int id)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> FindUserByUserName(string userName)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }


        public async Task<User> AddUser(UserForManipulation user)
        {
            User newUser = new User { UserName = user.UserName, FullName = user.FullName };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
            return newUser;
        }

        public async Task UpdateUser(UserForManipulation newUser, User oldUser)
        {
            oldUser.UserName = newUser.UserName;
            oldUser.FullName = newUser.FullName;
            await db.SaveChangesAsync();        
        }

        public async Task<bool> DeleteUser(User user)
        {
            if (await db.HoursRecords.AnyAsync(u => u.User.UserId == user.UserId))
            {
                return false;
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
