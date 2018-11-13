using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace KolotreeWebApi.Models
{
    public class ReportService
    {     

        private readonly KolotreeContext db;
        private readonly UserService userService;
        private readonly ProjectService projectService;
        private readonly HoursRecordService hoursRecordService;
        

        public ReportService(KolotreeContext _db, UserService _userService, ProjectService _projectService, HoursRecordService _hoursRecordService)
        {
            db = _db;
            userService = _userService;
            projectService = _projectService;
            hoursRecordService = _hoursRecordService;
        }

        private List<SimpleProjectReport> GetSimpleProjectReports(IEnumerable<HoursRecord> hoursRecords)
        {
            if (hoursRecords == null)
            {
                return null;
            }
            List<SimpleProjectReport> resultReport = new List<SimpleProjectReport>();
            var recordsGroupedByProject = hoursRecords.GroupBy(r => r.Project);

            foreach (var group in recordsGroupedByProject)
            {
                var report = new SimpleProjectReport();
                report.Project = group.Key;
                report.AssignedHours = group.Sum(r => r.AssignedHours);
                report.SpentHours = group.Sum(r => r.SpentHours);
                resultReport.Add(report);
            }
            return resultReport;
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
            IEnumerable<HoursRecord> userOnProjectList = hoursRecordService.hoursRecords
               .Where(us => (us.UserId == user.UserId) &&
               (us.Date >= startDate && us.Date <= endDate));
          
            ReportPerUser resultReport = new ReportPerUser();
            resultReport.User = user;
            resultReport.Projects = GetSimpleProjectReports(userOnProjectList);
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

            IEnumerable<HoursRecord> userOnProjectList = hoursRecordService.hoursRecords
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

        private List<SimpleUserReport> GetSimpleUserReports(IEnumerable<HoursRecord> hoursRecords)
        {
            if (hoursRecords == null)
            {
                return null;
            }
            List<SimpleUserReport> resultReport = new List<SimpleUserReport>();
            var recordsGroupedByUser = hoursRecords.GroupBy(r => r.User);

            foreach (var group in recordsGroupedByUser)
            {
                SimpleUserReport report = new SimpleUserReport();
                report.User = group.Key;
                report.AssignedHours = group.Sum(r => r.AssignedHours);
                report.SpentHours = group.Sum(r => r.SpentHours);
                resultReport.Add(report);
            }
            return resultReport;
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
            IEnumerable<HoursRecord> recordHoursList = hoursRecordService.hoursRecords
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
            IEnumerable<HoursRecord> recordHours = hoursRecordService.hoursRecords
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
            IEnumerable<HoursRecord> recordHours = hoursRecordService.hoursRecords
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
