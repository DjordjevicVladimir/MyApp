using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ReportPerProjectService
    {
        private UserService userService = new UserService();
        private ProjectService projectService = new ProjectService();
        private UserOnProjectService userOnProjectService = new UserOnProjectService();

        //private List<UsersForProject> GetUsersForProject(List<UserOnProject> userOnProjectList)
        //{
        //    if (userOnProjectList == null)
        //    {
        //        return new List<ReportPerProject.UsersForProject>();
        //    }
        //    List<ReportPerProject.UsersForProject> usersOnProject = new List<ReportPerProject.UsersForProject>();
        //    HashSet<User> users = new HashSet<User>();
        //    foreach (var rec in userOnProjectList)
        //    {
        //        User user = userService.FindUser(rec.UserId);
        //        if (users.Contains(user))
        //        {
        //            ReportPerProject.UsersForProject existingUserOnProject = usersOnProject.FirstOrDefault(u => u.User.UserId == user.UserId);
        //            if (rec.Hours > 0)
        //            {
        //                existingUserOnProject.AssignedHours += rec.Hours;
        //            }
        //            else
        //            {
        //                existingUserOnProject.SpentHours += rec.Hours;
        //            }
        //        }
        //        else
        //        {
        //            users.Add(user);
        //            ReportPerProject.UsersForProject newUserOnProject = new ReportPerProject.UsersForProject();
        //            newUserOnProject.User = user;
        //            if (rec.Hours > 0)
        //            {
        //                newUserOnProject.AssignedHours += rec.Hours;
        //            }
        //            else
        //            {
        //                newUserOnProject.SpentHours += rec.Hours;
        //            }
        //            usersOnProject.Add(newUserOnProject);
        //        }
        //    }
        //    return usersOnProject;
        //}


        //public ReportPerProject GetReportPerProject()



    }
}
