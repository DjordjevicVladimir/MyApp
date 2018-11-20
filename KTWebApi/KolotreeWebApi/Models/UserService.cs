using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public User FindUserSinh(int id)
        {
            return  db.Users.FirstOrDefault(u => u.UserId == id);
        }


        public async Task<User> FindUser(int id)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }


        public async Task<User> AddUser(UserForManipulation user)
        {
            User newUser = new User { UserName = user.UserName, FullName = user.FullName };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
            return newUser;
        }

        public async void UpdateUser(UserForManipulation newUser, User oldUser)
        {
            oldUser.UserName = newUser.UserName;
            oldUser.FullName = newUser.FullName;
            await db.SaveChangesAsync();

        }

        public async Task DeleteUser(User user)
        {

            //List<HoursRecord> hoursRecords = db.HoursRecords.ToList();
            //if (hoursRecords != null)
            //{
            //    foreach (var record in hoursRecords)
            //    {
            //        db.HoursRecords.Remove(record);
            //    }
            //}

            //db.Users.Remove(user);
            //await db.SaveChangesAsync();
        }
    }
}
