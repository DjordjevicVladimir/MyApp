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
            ProjectService projectService = new ProjectService();
            UserOnProjectService userOnProjectService = new UserOnProjectService();
            ReportPerUserService rpus = new ReportPerUserService();

            User u = userService.FindUser(1);

            ReportPerUser ru = rpus.GetReportPerUser(u);
            Console.WriteLine(ru.User.FullName);
            foreach (var item in ru.ProjectsForUser)
            {
                Console.WriteLine("\t" + item.Project.Name + " " + item.AssignedHours + " " + item.SpentHours + " " + item.TotalHours);
            }
            Console.WriteLine("Total assigned hours = " + ru.TotalAssignedHours);
            Console.WriteLine("Total spent hours = " + ru.TotalSpentHours);
            Console.WriteLine("Total  hours = " + ru.TotalHours);


            Project p = projectService.FindProject(2);
            ReportPerProject rp = rpus.GetReportPerProject(p);

            Console.WriteLine(rp.Project.Name);
            foreach (var user in rp.UsersForProject)
            {
                Console.WriteLine("\t" + " " + user.User.FullName + " " + user.AssignedHours + " " + user.SpentHours + " " + user.TotalHours);
            }
            Console.WriteLine("Total assigned hours = " + rp.TotalAssignedHours);
            Console.WriteLine("Total spent hours = " + rp.TotalSpentHours);
            Console.WriteLine("Total  hours = " + rp.TotalHours);
            Console.Read();
        }
    }
}
