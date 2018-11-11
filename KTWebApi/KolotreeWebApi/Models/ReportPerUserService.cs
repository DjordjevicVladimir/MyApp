using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace KolotreeWebApi.Models
{
    public class ReportPerUserService
    {

        private UserService userService = new UserService();
        private ProjectService projectService = new ProjectService();
        private UserOnProjectService userOnProjectService = new UserOnProjectService();

        private List<ReportPerUser.ProjectForUser> GetProjectForUsers(List<UserOnProject> userOnProjectList)
        {
            List<ReportPerUser.ProjectForUser> records = new List<ReportPerUser.ProjectForUser>();
            foreach (var rec in userOnProjectList)
            {
                ReportPerUser.ProjectForUser project = new ReportPerUser.ProjectForUser();
                project.Project = projectService.FindProject(rec.ProjectId);
               
                if (rec.Hours > 0)
                {
                    project.AssignedHours = rec.Hours;
                }
                else
                {
                    project.SpentHours = rec.Hours;
                }               
                records.Add(project);
            }
            return records;
        }

       
        public ReportPerUser GetReportForUsersAllProjects(User user,  DateTime? fromDate, DateTime? toDate )
        {
            if (userService.FindUser(user.UserId) == null)
            {
                return new ReportPerUser();
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerUser();
            }          
            List<UserOnProject> userOnProjectList = userOnProjectService._ListOfUsersOnProjects
               .Where(us => (us.UserId == user.UserId) && 
               (us.Date >= startDate && us.Date <= endDate)).ToList();
            ReportPerUser report = new ReportPerUser();
            report.User = user;
            report.ProjectsForUser = GetProjectForUsers(userOnProjectList);
            report.FromDate = startDate;
            report.ToDate = endDate;
            return report;
        }

        public ReportPerUser GetReportForUserOnProject(User user,Project project, DateTime? fromDate, DateTime? toDate)
        {
            if (userService.FindUser(user.UserId) == null)
            {
                return new ReportPerUser();
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerUser();
            }
            if (projectService.FindProject(project.ProjectId)== null)
            {
                return new ReportPerUser();
            }

            List<UserOnProject> userOnProjectList = userOnProjectService._ListOfUsersOnProjects
                .Where(us => (us.UserId == user.UserId) && (us.ProjectId == project.ProjectId) &&
                (us.Date >= startDate && us.Date <= endDate)).ToList();
            ReportPerUser report = new ReportPerUser
            {
                User = user,
                FromDate = startDate,
                ToDate = endDate,
                ProjectsForUser = GetProjectForUsers(userOnProjectList)
            };
            return report;
        }

    }
}
