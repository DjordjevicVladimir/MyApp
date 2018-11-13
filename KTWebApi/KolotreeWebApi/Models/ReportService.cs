using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace KolotreeWebApi.Models
{
    public class ReportService
    {

        private UserService userService = new UserService();
        private ProjectService projectService = new ProjectService();
        private HoursRecordService userOnProjectService = new HoursRecordService();

        private List<SimpleProjectReport> GetSimpleProjectReports(IEnumerable<HoursRecord> userOnProjectList)
        {
            if (userOnProjectList == null)
            {
                return new List<SimpleProjectReport>();
            }
            List<SimpleProjectReport> projectsForUser = new List<SimpleProjectReport>();
            HashSet<Project> projects = new HashSet<Project>();
            foreach (var rec in userOnProjectList)
            {
                Project p = projectService.FindProject(rec.ProjectId);
                if (projects.Contains(p))
                {
                    SimpleProjectReport existingProject = projectsForUser.FirstOrDefault(pr => pr.Project.ProjectId == p.ProjectId);
                  
                        existingProject.AssignedHours += rec.AssignedHours;
                   
                        existingProject.SpentHours += rec.SpentHours;
                   
                    existingProject.TotalHours = existingProject.AssignedHours - existingProject.SpentHours;
                }
                else
                {                   
                    projects.Add(p);
                    SimpleProjectReport newProject = new SimpleProjectReport();
                    newProject.Project = p;                    
                    newProject.AssignedHours = rec.AssignedHours;                  
                    newProject.SpentHours = rec.SpentHours;                    
                    newProject.TotalHours = newProject.AssignedHours - newProject.SpentHours;
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
            IEnumerable<HoursRecord> userOnProjectList = userOnProjectService.recordsList
               .Where(us => (us.UserId == user.UserId) && 
               (us.Date >= startDate && us.Date <= endDate));
            ReportPerUser report = new ReportPerUser();
            report.User = user;
            report.Projects = GetSimpleProjectReports(userOnProjectList);
            foreach (var project in report.Projects)
            {
                report.TotalAssignedHours += project.AssignedHours;
                report.TotalSpentHours += project.SpentHours;
            }
            report.TotalHours = report.TotalAssignedHours - report.TotalSpentHours;
            report.FromDate = startDate;
            report.ToDate = endDate;
            return report;
        }

        public ReportPerUserOnProject GetReportPerUserOnProject(User user,Project project, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (userService.FindUser(user.UserId) == null)
            {
                return new ReportPerUserOnProject();
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerUserOnProject();
            }
            if (projectService.FindProject(project.ProjectId)== null)
            {
                return new ReportPerUserOnProject();
            }

            IEnumerable<HoursRecord> userOnProjectList = userOnProjectService.recordsList
                .Where(us => (us.UserId == user.UserId) && (us.ProjectId == project.ProjectId) &&
                (us.Date >= startDate && us.Date <= endDate));
            ReportPerUserOnProject report = new ReportPerUserOnProject();
            report.User = user;
            report.Project = project;            
            foreach (var rec in userOnProjectList)
            {
                report.AssignedHours += rec.AssignedHours;
                report.SpentHours += rec.SpentHours;                
            }
            report.TotalHours = report.AssignedHours - report.SpentHours;   
            return report;
        }

        private List<SimpleUserReport> GetSimpleUserReports(IEnumerable<HoursRecord> userOnProjectList)
        {
            if (userOnProjectList == null)
            {
                return new List<SimpleUserReport>();
            }
            List<SimpleUserReport> usersOnProject = new List<SimpleUserReport>();
            HashSet<User> users = new HashSet<User>();
            foreach (var rec in userOnProjectList)
            {
                User user = userService.FindUser(rec.UserId);
                if (users.Contains(user))
                {
                    SimpleUserReport existingUserOnProject = usersOnProject.FirstOrDefault(u => u.User.UserId == user.UserId);
                    existingUserOnProject.AssignedHours += rec.AssignedHours;
                    existingUserOnProject.SpentHours += rec.SpentHours;
                    existingUserOnProject.TotalHours = existingUserOnProject.AssignedHours - existingUserOnProject.SpentHours;
                }
                else
                {
                    users.Add(user);
                    SimpleUserReport newUserOnProject = new SimpleUserReport();
                    newUserOnProject.User = user;
                    newUserOnProject.AssignedHours = rec.AssignedHours;
                    newUserOnProject.SpentHours = rec.SpentHours ;
                    newUserOnProject.TotalHours = newUserOnProject.AssignedHours - newUserOnProject.SpentHours;
                    usersOnProject.Add(newUserOnProject);
                }
            }
            return usersOnProject;
        }

        public ReportPerProject GetReportPerProject(Project project, DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerProject();
            }
            if (projectService.FindProject(project.ProjectId) == null)
            {
                return new ReportPerProject();
            }

            ReportPerProject resultReport = new ReportPerProject();
            resultReport.Project = project;
            IEnumerable<HoursRecord> recordHoursList = userOnProjectService.recordsList
               .Where(r => (r.ProjectId == project.ProjectId) &&
               (r.Date >= startDate && r.Date <= endDate));
            resultReport.Users = GetSimpleUserReports(recordHoursList);
            foreach (var user in resultReport.Users)
            {
                resultReport.TotalAssignedHours += user.AssignedHours;
                resultReport.TotalSpentHours += user.SpentHours;
            }
            resultReport.TotalHours = resultReport.TotalAssignedHours + resultReport.TotalSpentHours;
            resultReport.FromDate = startDate;
            resultReport.ToDate = endDate;
            return resultReport;

        }

        public SumProjectsReport GetSumProjectsReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new SumProjectsReport();
            }            
            SumProjectsReport resultReport = new SumProjectsReport();
            IEnumerable<HoursRecord> recordHours = userOnProjectService.recordsList
                .Where(r => (r.Date >= startDate) && (r.Date <= endDate));
            resultReport.Projects = GetSimpleProjectReports(recordHours);
            foreach (var project in resultReport.Projects)
            {
                resultReport.TotalAssignedHours += project.AssignedHours;
                resultReport.TotalSpentHours += project.SpentHours;
            }
            resultReport.TotalHours = resultReport.TotalAssignedHours - resultReport.TotalSpentHours;
            resultReport.FromDate = startDate;
            resultReport.ToDate = endDate;

            return resultReport;            
             
        }

        public SumUsersReport GetSumUsersReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new SumUsersReport();
            }
            SumUsersReport resultReport = new SumUsersReport();
            IEnumerable<HoursRecord> recordHours = userOnProjectService.recordsList
               .Where(r => (r.Date >= startDate) && (r.Date <= endDate));
            resultReport.users = GetSimpleUserReports(recordHours);
            foreach (var user in resultReport.users)
            {
                resultReport.TotalAssignedHours += user.AssignedHours;
                resultReport.TotalSpentHours += user.SpentHours;                
            }
            resultReport.TotalSpentHours = resultReport.TotalAssignedHours - resultReport.TotalSpentHours;
            resultReport.FromDate = startDate;
            resultReport.ToDate = endDate;
            return resultReport;
        }


    }
}
