using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ReportService
    {

        private UserOnProjectService _userOnProjectService = new UserOnProjectService();
        private UserService _userService = new UserService();
        private ProjectService _projectService = new ProjectService();
        

        public List<Report> GetAllRecordsForUserOnProject(int userId, int projectId)
        {
            Project project = _projectService.FindProject(projectId);
            if (project == null)
            {
                return null;
            }
            User user = _userService.FindUser(userId);
            if (user == null)
            {
                return null;
            }
            List<UserOnProject> listOfRecords = _userOnProjectService._ListOfUsersOnProjects
                .Where(us => (us.UserId == userId) && (us.ProjectId == projectId)).ToList();
            List<Report> reportList = new List<Report>();
            foreach (var rec in listOfRecords)
            {
                Report r = new Report();
                r.User = user;
                r.Project = project;
                if (rec.Hours > 0)
                {
                    r.AssignedHours = rec.Hours;
                }
                else
                {
                    r.SpentHours = rec.Hours;
                }
                r.DateOfRecord = rec.Date;
                reportList.Add(r);
            }
            return reportList;
            
        }

        public List<Report> GetAllRecordsForUserOnProject(int userId, int projectId, DateTime startDate, DateTime endDate)
        {
            Project project = _projectService.FindProject(projectId);
            if (project == null)
            {
                return null;
            }
            User user = _userService.FindUser(userId);
            if (user == null)
            {
                return null;
            }
            List<UserOnProject> listOfRecords = _userOnProjectService._ListOfUsersOnProjects
                .Where(us => (us.UserId == userId) && (us.ProjectId == projectId) &&
                (us.Date >= startDate && us.Date <= endDate )).ToList();
            List<Report> reportList = new List<Report>();
            foreach (var rec in listOfRecords)
            {
                Report r = new Report();
                r.User = user;
                r.Project = project;
                if (rec.Hours > 0)
                {
                    r.AssignedHours = rec.Hours;
                }
                else
                {
                    r.SpentHours = rec.Hours;
                }
                r.DateOfRecord = rec.Date;
                reportList.Add(r);
            }
            return reportList;

        }

        
    }
}
