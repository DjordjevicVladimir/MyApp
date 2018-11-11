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

        private List<ProjectForUser> GetProjectsForUsers(List<UserOnProject> userOnProjectList)
        {
            if (userOnProjectList == null)
            {
                return new List<ProjectForUser>();
            }
            List<ProjectForUser> projectsForUser = new List<ProjectForUser>();
            HashSet<Project> projects = new HashSet<Project>();
            foreach (var rec in userOnProjectList)
            {
                Project p = projectService.FindProject(rec.ProjectId);
                if (projects.Contains(p))
                {
                    ProjectForUser existingProject = projectsForUser.FirstOrDefault(pr => pr.Project.ProjectId == p.ProjectId);
                    if (rec.Hours > 0)
                    {
                        existingProject.AssignedHours += rec.Hours;
                    }
                    else
                    {
                        existingProject.SpentHours += rec.Hours;
                    }
                    existingProject.TotalHours = existingProject.AssignedHours + existingProject.SpentHours;
                }
                else
                {                   
                    projects.Add(p);
                    ProjectForUser newProject = new ProjectForUser();
                    newProject.Project = p;
                    if (rec.Hours > 0)
                    {
                        newProject.AssignedHours = rec.Hours;
                    }
                    else
                    {
                        newProject.SpentHours = rec.Hours;
                    }
                    newProject.TotalHours = newProject.AssignedHours + newProject.SpentHours;
                    projectsForUser.Add(newProject);
                }
                
            }
            
            return projectsForUser;
        }

                      
        public ReportPerUser GetReportPerUser(User user,  DateTime? fromDate = null, DateTime? toDate = null )
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
            report.ProjectsForUser = GetProjectsForUsers(userOnProjectList);
            foreach (var project in report.ProjectsForUser)
            {
                report.TotalAssignedHours += project.AssignedHours;
                report.TotalSpentHours += project.SpentHours;
            }
            report.TotalHours = report.TotalAssignedHours + report.TotalSpentHours;
            report.FromDate = startDate;
            report.ToDate = endDate;
            return report;
        }

        public ReportPerUser GetReportPerUserOnProject(User user,Project project, DateTime? fromDate = null, DateTime? toDate = null)
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
            ReportPerUser report = new ReportPerUser();
            report.User = user;
            report.ProjectsForUser = GetProjectsForUsers(userOnProjectList);
            foreach (var proj in report.ProjectsForUser)
            {
                report.TotalAssignedHours += proj.AssignedHours;
                report.TotalSpentHours += proj.SpentHours;
            }
            report.TotalHours = report.TotalAssignedHours - report.TotalSpentHours;   
            return report;
        }

        private List<UserForProject> GetUsersForProject(List<UserOnProject> userOnProjectList)
        {
            if (userOnProjectList == null)
            {
                return new List<UserForProject>();
            }
            List<UserForProject> usersOnProject = new List<UserForProject>();
            HashSet<User> users = new HashSet<User>();
            foreach (var rec in userOnProjectList)
            {
                User user = userService.FindUser(rec.UserId);
                if (users.Contains(user))
                {
                   UserForProject existingUserOnProject = usersOnProject.FirstOrDefault(u => u.User.UserId == user.UserId);
                    if (rec.Hours > 0)
                    {
                        existingUserOnProject.AssignedHours += rec.Hours;
                    }
                    else
                    {
                        existingUserOnProject.SpentHours += rec.Hours;
                    }
                }
                else
                {
                    users.Add(user);
                  UserForProject newUserOnProject = new UserForProject();
                    newUserOnProject.User = user;
                    if (rec.Hours > 0)
                    {
                        newUserOnProject.AssignedHours += rec.Hours;
                    }
                    else
                    {
                        newUserOnProject.SpentHours += rec.Hours;
                    }
                    usersOnProject.Add(newUserOnProject);
                }
            }
            return usersOnProject;
        }


        public ReportPerProject GetReportPerProject(Project project, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (projectService.FindProject(project.ProjectId) == null)
            {
                return new ReportPerProject();
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerProject();
            }
            List<UserOnProject> userOnProjectList = userOnProjectService._ListOfUsersOnProjects
               .Where(us => (us.ProjectId == project.ProjectId) &&
               (us.Date >= startDate && us.Date <= endDate)).ToList();
            ReportPerProject report = new ReportPerProject();
            report.Project = project;
            report.UsersForProject = GetUsersForProject(userOnProjectList);
            foreach (var user in report.UsersForProject)
            {
                report.TotalAssignedHours += user.AssignedHours;
                report.TotalSpentHours += user.SpentHours;
            }
            report.TotalHours = report.TotalAssignedHours + report.TotalSpentHours;
            report.FromDate = startDate;
            report.ToDate = endDate;
            return report;
        }

    }
}
