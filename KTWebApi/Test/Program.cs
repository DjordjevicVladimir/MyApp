using System;
using System.Collections.Generic;
using KolotreeWebApi;
using KolotreeWebApi.Models;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            UserService userService = new UserService();
            List<User> users = userService.FetchAllUsers();
            User u = new User { UserId = 4, FullName = "ivan", UserName = "ivan" };
            userService.AddUser(u);
            User u1 = new User { UserId = 2, UserName = "acacaca" };
            userService.UpdateUser(u1);
            foreach (var user in users)
            {
                Console.WriteLine(user.UserName);
            }

            ProjectService ps = new ProjectService();
            List<Project> projects = ps.FetchAllProjects();
            Project p1 = new Project { ProjectId = 3, Name = "myproject", Description = "sdasdadasd" };
            ps.AddProject(p1);
            Project p2 = new Project { ProjectId = 2, Name = "dasdada" };
            ps.UpdateProject(p2);
            foreach (var p in projects)
            {
                Console.WriteLine(p.Name);
            }


            Console.Read();
        }
    }
}
