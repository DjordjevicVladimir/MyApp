using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class UserService
    {
        private readonly KolotreeContext db;       

        public UserService(KolotreeContext _db)
        {
            db = _db;
        }


        public  async Task<List<User>> FetchAllUsers()
        {           
            return  await db.Users.ToListAsync();
        }


        public async Task<User> FindUser(int id)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }


        public  async Task AddUser(User user)
        {
             db.Users.Add(user);
            await db.SaveChangesAsync();                           
        }

        public async void UpdateUser(User user)
        {
            User oldUser = await db.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);          
            oldUser.UserName = user.UserName;
            oldUser.FullName = user.FullName;            
            await db.SaveChangesAsync();         
       
        }

        public async Task DeleteUser(User user)
        {
                     
            List<HoursRecord> hoursRecords = user.HoursRecords.ToList();
            if (hoursRecords != null)
            {
                foreach (var record in hoursRecords)
                {
                    db.HoursRecords.Remove(record);
                }
            }

            db.Users.Remove(user);
           await db.SaveChangesAsync();     
        }
    }
}
