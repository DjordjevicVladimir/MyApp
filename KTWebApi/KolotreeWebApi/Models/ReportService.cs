using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KolotreeWebApi.Models
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

        private async Task<List<SimpleProjectReport>> GetSimpleProjectReports(IEnumerable<HoursRecord> hoursRecords)
        {
            if (hoursRecords == null)
            {
                return null;
            }
            List<SimpleProjectReport> resultReport = new List<SimpleProjectReport>();
            var recordsGroupedByProject = hoursRecords.GroupBy(r => r.ProjectId);

            foreach (var group in recordsGroupedByProject)
            {
                var report = new SimpleProjectReport();
                report.Project = await projectService.FindProject(group.Key);
                report.AssignedHours = group.Sum(r => r.AssignedHours);
                report.SpentHours = group.Sum(r => r.SpentHours);
                report.TotalHours = report.AssignedHours - report.SpentHours;
                resultReport.Add(report);
            }
            return resultReport;
        }


        public async Task<ReportPerUser> GetReportPerUser(User user, DateTime startDate, DateTime endDate)
        {

            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> userOnProject = hoursRecords.Where(us => us.UserId == user.UserId
            && us.Date >= startDate.Date && us.Date <= endDate.Date);

            ReportPerUser resultReport = new ReportPerUser();
            resultReport.User = user;
            resultReport.Projects = await GetSimpleProjectReports(userOnProject);
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

        public async Task<ReportPerUserOnProject> GetReportPerUserOnProject(User user, Project project, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (await userService.FindUser(user.UserId) == null)
            {
                return new ReportPerUserOnProject();
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerUserOnProject();
            }
            if (projectService.FindProject(project.ProjectId) == null)
            {
                return new ReportPerUserOnProject();
            }
            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> userOnProject = hoursRecords.Where(us => (us.UserId == user.UserId)
            && (us.ProjectId == project.ProjectId)
            && (us.Date >= startDate && us.Date <= endDate));
            ReportPerUserOnProject report = new ReportPerUserOnProject();
            report.User = user;
            report.Project = project;
            foreach (var rec in userOnProject)
            {
                report.AssignedHours += rec.AssignedHours;
                report.SpentHours += rec.SpentHours;
            }
            report.TotalHours = report.AssignedHours - report.SpentHours;
            return report;
        }

        private async Task<List<SimpleUserReport>> GetSimpleUserReports(IEnumerable<HoursRecord> hoursRecords)
        {
            if (hoursRecords == null)
            {
                return null;
            }
            List<SimpleUserReport> resultReport = new List<SimpleUserReport>();
            var recordsGroupedByUser = hoursRecords.GroupBy(r => r.UserId);

            foreach (var group in recordsGroupedByUser)
            {
                SimpleUserReport report = new SimpleUserReport();
                report.User = await userService.FindUser(group.Key);
                report.AssignedHours = group.Sum(r => r.AssignedHours);
                report.SpentHours = group.Sum(r => r.SpentHours);
                resultReport.Add(report);
            }
            return resultReport;
        }



        public async Task<ReportPerProject> GetReportPerProject(Project project, DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerProject();
            }
            if (await projectService.FindProject(project.ProjectId) == null)
            {
                return new ReportPerProject();
            }

            ReportPerProject resultReport = new ReportPerProject();
            resultReport.Project = project;
            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> recordHoursList = hoursRecords.Where(r => (r.ProjectId == project.ProjectId)
            && (r.Date >= startDate && r.Date <= endDate));
            resultReport.Users = await GetSimpleUserReports(recordHoursList);
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

        public async Task<SumProjectsReport> GetSumProjectsReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new SumProjectsReport();
            }
            SumProjectsReport resultReport = new SumProjectsReport();
            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> recordHours = hoursRecords.Where(r => (r.Date >= startDate)
                && (r.Date <= endDate));
            resultReport.Projects = await GetSimpleProjectReports(recordHours);
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

        public async Task<SumUsersReport> GetSumUsersReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new SumUsersReport();
            }
            SumUsersReport resultReport = new SumUsersReport();
            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> recordHours = hoursRecords.Where(r => (r.Date >= startDate)
                && (r.Date <= endDate));
            resultReport.users = await GetSimpleUserReports(recordHours);
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


        public async Task<ReportPerUsersSpentHoursRecords> GetUsersSpentHoursRecords(User user, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (await userService.FindUser(user.UserId) == null)
            {
                return new ReportPerUsersSpentHoursRecords();
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerUsersSpentHoursRecords();
            }
            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> hoursRecordsForUser = hoursRecords.Where(r => (r.User == user) 
            && (r.Date >= startDate && r.Date <= endDate));
            ReportPerUsersSpentHoursRecords resultReport = new ReportPerUsersSpentHoursRecords();
            resultReport.User = user;
            foreach (var rec in hoursRecords)
            {
                var record = new ReportPerUsersSpentHoursRecords.SpentHoursRecord();
                record.Project = rec.Project;
                if (rec.SpentHours > 0)
                {
                    record.SpentHours = rec.SpentHours;
                }
                record.DateOfRecord = rec.Date;
                resultReport.spentHoursRecords.Add(record);
            }
            resultReport.FromDate = startDate;
            resultReport.ToDate = endDate;
            return resultReport;
        }


        public async  Task<ReportPerProjectRecords> GetReportPerProjectRecords(Project project, DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (fromDate > toDate)
            {
                return new ReportPerProjectRecords();
            }
            if (await projectService.FindProject(project.ProjectId) == null)
            {
                return new ReportPerProjectRecords();
            }
            ReportPerProjectRecords resultReport = new ReportPerProjectRecords();
            resultReport.Project = project;
            IEnumerable<HoursRecord> hoursRecords = await hoursRecordService.GetAllHoursRecordsAsync();
            IEnumerable<HoursRecord> recordHoursList = hoursRecords.Where(r => (r.ProjectId == project.ProjectId) 
            && (r.Date >= startDate && r.Date <= endDate));
            foreach (var rec in recordHoursList)
            {
                var record = new ReportPerProjectRecords.SpentHoursRecord();
                record.User = rec.User;
                if (rec.SpentHours > 0)
                {
                    record.SpentHours = rec.SpentHours;
                }
                record.DateOfRecord = rec.Date;
                resultReport.spentHoursRecords.Add(record);
            }
            resultReport.FromDate = startDate;
            resultReport.ToDate = endDate;
            return resultReport;
        }


    }
}
