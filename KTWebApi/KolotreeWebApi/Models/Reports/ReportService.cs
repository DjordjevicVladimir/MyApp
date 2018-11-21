using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KolotreeWebApi.Models.Reports
{
    public class ReportService
    {
        private readonly UserService userService;
        private readonly ProjectService projectService;
        private readonly HoursRecordService hoursRecordService;


        public ReportService(UserService _userService, ProjectService _projectService, HoursRecordService _hoursRecordService)
        {
            userService = _userService;
            projectService = _projectService;
            hoursRecordService = _hoursRecordService;
        }

        private List<ProjectsHelperReport> GetProjectsHelperReports(IEnumerable<HoursRecord> hoursRecords)
        {
            if (hoursRecords == null)
            {
                return new List<ProjectsHelperReport>();
            }
            List<ProjectsHelperReport> resultReport = new List<ProjectsHelperReport>();
            var recordsGroupedByProject = hoursRecords.GroupBy(r => r.Project);

            foreach (var group in recordsGroupedByProject)
            {
                var report = new ProjectsHelperReport();
                report.Project = group.Key;
                report.AssignedHours = group.Sum(r => r.AssignedHours);
                report.SpentHours = group.Sum(r => r.SpentHours);
                report.AvailableHours = report.AssignedHours - report.SpentHours;
                resultReport.Add(report);
            }
            return resultReport;
        }


        public async Task<UserReport> GetUserReport(User user, DateTime startDate, DateTime endDate)
        {  
          
            UserReport resultReport = new UserReport();
            resultReport.User = user;
            IEnumerable<HoursRecord> hoursRecordsForUser = await hoursRecordService.FindRecordsByUserAndDateRange(user, startDate, endDate);
            resultReport.UserWorkOnProjects = GetProjectsHelperReports(hoursRecordsForUser);
            foreach (var project in resultReport.UserWorkOnProjects)
            {
                resultReport.AssignedHoursSum += project.AssignedHours;
                resultReport.SpentHoursSum += project.SpentHours;
            }
            resultReport.AvailableHoursSum = resultReport.AssignedHoursSum - resultReport.SpentHoursSum;
            resultReport.FromDate = startDate;
            resultReport.ToDate = endDate;
            return resultReport;
        }

        public async Task<UserOnProjectReport> GetReportForUserOnProject(User user, Project project, DateTime fromDate, DateTime toDate)
        {           
           
            UserOnProjectReport report = new UserOnProjectReport();
            report.User = user;
            report.Project = project;
            report.FromDate = fromDate;
            report.ToDate = toDate;
            IEnumerable<HoursRecord> hoursRecordsForUserOnProject =
               await hoursRecordService.FindRecordsByUserAndProjectAndDateRange(user, project, fromDate, toDate);
            foreach (var rec in hoursRecordsForUserOnProject)
            {
                report.AssignedHours += rec.AssignedHours;
                report.SpentHours += rec.SpentHours;
            }
            report.AvailableHours = report.AssignedHours - report.SpentHours;
            return report;
        }

        private List<UserHelperReport> GetUserHelperReports(IEnumerable<HoursRecord> hoursRecords)
        {
            if (hoursRecords == null)
            {
                return new List<UserHelperReport>();
            }
            List<UserHelperReport> resultReport = new List<UserHelperReport>();
            var recordsGroupedByUser = hoursRecords.GroupBy(r => r.User);
            foreach (var group in recordsGroupedByUser)
            {
                UserHelperReport report = new UserHelperReport();
                report.User = group.Key;
                report.AssignedHours = group.Sum(r => r.AssignedHours);
                report.SpentHours = group.Sum(r => r.SpentHours);
                report.AvailableHours = report.AssignedHours - report.SpentHours;
                resultReport.Add(report);
            }
            return resultReport;
        }



        public async Task<ProjectReport> GetProjectReport(Project project, DateTime fromDate, DateTime toDate)
        {            
            ProjectReport resultReport = new ProjectReport();
            resultReport.Project = project;
            IEnumerable<HoursRecord> recordForProject =
                await hoursRecordService.FindRecordsByProjectAndDateRange(project, fromDate, toDate);
            resultReport.UsersWorkOnProject =  GetUserHelperReports(recordForProject);
            foreach (var user in resultReport.UsersWorkOnProject)
            {
                resultReport.AssignedHoursSum += user.AssignedHours;
                resultReport.SpentHoursSum += user.SpentHours;
            }
            resultReport.AvailableHoursSum = resultReport.AssignedHoursSum - resultReport.SpentHoursSum;
            resultReport.FromDate = fromDate;
            resultReport.ToDate = toDate;
            return resultReport;

        }

        public async Task<SumProjectsReport> GetSumProjectsReport(DateTime fromDate , DateTime toDate )
        {
            
            SumProjectsReport resultReport = new SumProjectsReport();
            IEnumerable<HoursRecord> recordHours = await hoursRecordService.FindRecordsByDateRange(fromDate, toDate);
            resultReport.Projects = GetProjectsHelperReports(recordHours);
            foreach (var project in resultReport.Projects)
            {
                resultReport.AssignedHoursSum += project.AssignedHours;
                resultReport.SpentHoursSum += project.SpentHours;
            }
            resultReport.AvailableHoursSum = resultReport.AssignedHoursSum - resultReport.SpentHoursSum;
            resultReport.FromDate = fromDate;
            resultReport.ToDate = toDate;

            return resultReport;

        }

        public async Task<SumUsersReport> GetSumUsersReport(DateTime fromDate, DateTime toDate)
        {            
            SumUsersReport resultReport = new SumUsersReport();
            IEnumerable<HoursRecord> recordHours = await hoursRecordService.FindRecordsByDateRange(fromDate, toDate);
            resultReport.Users =  GetUserHelperReports(recordHours);
            foreach (var user in resultReport.Users)
            {
                resultReport.AssignedHoursSum += user.AssignedHours;
                resultReport.SpentHoursSum += user.SpentHours;
            }
            resultReport.AvailableHoursSum = resultReport.AssignedHoursSum - resultReport.SpentHoursSum;
            resultReport.FromDate = fromDate;
            resultReport.ToDate = toDate;
            return resultReport;
        }


        public async Task<UserSpentHoursReport> GetUsersSpentHoursReport(User user, DateTime fromDate, DateTime toDate )
        {     
                    
            UserSpentHoursReport resultReport = new UserSpentHoursReport();
            resultReport.User = user;
            IEnumerable<HoursRecord> hoursRecordsForUser = 
                await hoursRecordService.FindRecordsByUserAndDateRange(user, fromDate, toDate);
            foreach (var rec in hoursRecordsForUser)
            {             
                if (rec.SpentHours > 0)
                {
                    var record = new UserSpentHoursReport.SpentHoursRecord();
                    record.Project = rec.Project;
                    record.SpentHours = rec.SpentHours;
                    record.DateOfRecord = rec.Date;
                    resultReport.spentHoursRecords.Add(record);
                }              
            }
            if (resultReport.spentHoursRecords == null)
            {
                resultReport.spentHoursRecords = new List<UserSpentHoursReport.SpentHoursRecord>();
            }
            resultReport.FromDate = fromDate;
            resultReport.ToDate = toDate;
            return resultReport;
        }


        public async  Task<ProjectSpentHoursReport> GetProjectSpentHoursReport(Project project, DateTime fromDate, DateTime toDate)
        {
           
            ProjectSpentHoursReport resultReport = new ProjectSpentHoursReport();
            resultReport.Project = project;

            IEnumerable<HoursRecord> hoursRecordsForProject =
                await hoursRecordService.FindRecordsByProjectAndDateRange(project, fromDate, toDate);
            foreach (var rec in hoursRecordsForProject)
            {              
                if (rec.SpentHours > 0)
                {
                    var record = new ProjectSpentHoursReport.SpentHoursRecord();
                    record.User = rec.User;
                    record.SpentHours = rec.SpentHours;
                    record.DateOfRecord = rec.Date;
                    resultReport.spentHoursRecords.Add(record);
                }               
            }
            if (resultReport.spentHoursRecords == null)
            {
                resultReport.spentHoursRecords = new List<ProjectSpentHoursReport.SpentHoursRecord>();
            }
            resultReport.FromDate = fromDate;
            resultReport.ToDate = toDate;
            return resultReport;
        }


    }
}
