using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class UserService
    {
        public  List<User> userList = new List<User> {
            new User{ UserId = 1, UserName= "Vlada", FullName = "Vlada" },
            new User{ UserId = 2, UserName= "aca", FullName = "aca"},
            new User{ UserId = 3, UserName= "nikola", FullName = "nikola"}
        };

        public  List<User> FetchAllUsers()
        {
            return userList;
        }

        public User FindUser(int id)
        {
            return userList.FirstOrDefault(u => u.UserId == id);
        }

        public void AddUser(User user)
        {
            if (user != null)
            {
                userList.Add(user);
            }          
        }

        public void UpdateUser(User user)
        {
            User oldUser = userList.FirstOrDefault(u => u.UserId == user.UserId);
            if (oldUser != null)
            {
                oldUser.UserName = user.UserName;
                oldUser.FullName = user.FullName;                
            }
        }
    }
}
