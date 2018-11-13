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
        public  List<User> FetchAllUsers()
        {
            return db.Users.ToList();
        }

        public User FindUser(int id)
        {
            return db.Users.FirstOrDefault(u => u.UserId == id);
        }

        public bool AddUser(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            User oldUser = db.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (oldUser == null)
            {
                return false;             
            }
            oldUser.UserName = user.UserName;
            oldUser.FullName = user.FullName;
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
