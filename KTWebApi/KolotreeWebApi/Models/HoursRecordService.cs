using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KolotreeWebApi.Models
{
    public class HoursRecordService
    {

        private readonly KolotreeDbContext db;
        private readonly UserService userService;
        private readonly ProjectService projectService;


        public HoursRecordService(KolotreeDbContext _db, UserService _userService, ProjectService _projectService)
        {
            db = _db;
            userService = _userService;
            projectService = _projectService;
        }              

        public async Task<HoursRecord> GetRacord(int id)
        {
           HoursRecord record = await db.HoursRecords.FirstOrDefaultAsync(r => r.Id == id);
            return record;
        }

        public async Task<HoursRecord> AddAssignedHoursToUserForProject(HoursRecordForCreation hoursRecord)
        {
            HoursRecord record = new HoursRecord();
            record.User = await userService.FindUser(hoursRecord.UserId);      
            record.Project = await projectService.FindProject(hoursRecord.ProjectId);       
            record.AssignedHours = hoursRecord.Hours;
            db.HoursRecords.Add(record);
            await db.SaveChangesAsync();
            return record;
        }

        public async Task<HoursRecord> RemoveAssignedHoursToUserForProject(HoursRecordForCreation hoursRecord)
        {
            HoursRecord record = new HoursRecord();
            record.User = await userService.FindUser(hoursRecord.UserId);
            record.Project = await projectService.FindProject(hoursRecord.ProjectId);
            record.AssignedHours -= hoursRecord.Hours;
            db.HoursRecords.Add(record);
            await db.SaveChangesAsync();
            return record;
        }


        public async Task<HoursRecord> AddSpentHoursToUserForProject(HoursRecordForCreation hoursRecord)
        {
            HoursRecord record = new HoursRecord();
            record.User = await userService.FindUser(hoursRecord.UserId);
            record.Project = await projectService.FindProject(hoursRecord.ProjectId);
            record.SpentHours = hoursRecord.Hours;
            db.HoursRecords.Add(record);
            await db.SaveChangesAsync();
            return record;
        }

        public async Task<List<HoursRecord>> FindRecordsByUserAndDateRange(User user, DateTime fromDate, DateTime toDate)
        {
            List<HoursRecord> recordForUserInDateRange = await db.HoursRecords.Where(us => us.User.UserId == user.UserId
           && us.Date >= fromDate && us.Date <= toDate).ToListAsync();
            return recordForUserInDateRange;
        }

        public async Task<List<HoursRecord>> FindRecordsByUserAndProjectAndDateRange(User user, Project project, DateTime fromDate, DateTime toDate)
        {
            List<HoursRecord> resultList = await db.HoursRecords.Where(us => (us.User.UserId == user.UserId)
            && (us.Project.ProjectId == project.ProjectId)
            && (us.Date >= fromDate && us.Date <= toDate)).ToListAsync();
            return resultList;
        }

        public async Task<List<HoursRecord>> FindRecordsByProjectAndDateRange(Project project, DateTime fromDate, DateTime toDate)
        {
            List<HoursRecord> recordForProjectInDateRange = await db.HoursRecords.Where(us => us.Project.ProjectId == project.ProjectId
           && us.Date >= fromDate && us.Date <= toDate).ToListAsync();
            return recordForProjectInDateRange;
        }

        public async Task<List<HoursRecord>> FindRecordsByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<HoursRecord> recordsByDateRange = await db.HoursRecords.Where(r => r.Date >= fromDate
                && r.Date <= toDate).ToListAsync();
            return recordsByDateRange;
        }

        public async Task<int> CheckAvailableHoursForUserOnProject(User user, Project project)
        {
            int totalAssignedHours = await db.HoursRecords.Where(r => (r.User == user) && (r.Project == project)).SumAsync(h => h.AssignedHours);
            int totalSpentHours = await db.HoursRecords.Where(r => (r.User == user) && (r.Project == project)).SumAsync(h => h.SpentHours);
            int availableHours = totalAssignedHours - totalSpentHours;
            return availableHours;
        }
    }
}
