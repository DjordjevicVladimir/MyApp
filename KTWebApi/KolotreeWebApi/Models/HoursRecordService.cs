using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KolotreeWebApi.Models
{
    public class HoursRecordService
    {
       
        private readonly KolotreeContext db;
        private readonly UserService userService;
        private readonly ProjectService projectService;

        public HoursRecordService(KolotreeContext _db, UserService _userService, ProjectService _projectService)
        {
            db = _db;
            userService = _userService;
            projectService = _projectService;
        }

        public async Task<List<HoursRecord>> GetAllHoursRecordsAsync()
        {
           return  await db.HoursRecords.ToListAsync();
        }

        public async Task<HoursRecord> FindHoursRecord(int id)
        {
            return await db.HoursRecords.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAssignedHoursToUserForProject(HoursRecordForCreation hoursRecord)
        {            
            HoursRecord record = new HoursRecord();
            record.User = await userService.FindUser(hoursRecord.UserId);
            record.Project = await projectService.FindProject(hoursRecord.ProjectId);
            record.AssignedHours = hoursRecord.Hours;
            db.HoursRecords.Add(record);
            await db.SaveChangesAsync();
        }

        public async Task RemoveAssignedHoursToUserForProject(HoursRecordForCreation hoursRecord)
        {
            HoursRecord record = new HoursRecord();
            record.User = await userService.FindUser(hoursRecord.UserId);
            record.Project = await projectService.FindProject(hoursRecord.ProjectId);
            record.AssignedHours -= hoursRecord.Hours;
            db.HoursRecords.Add(record);
            await db.SaveChangesAsync();
        }

        public async Task<int> CheckAvailableHoursForUserOnProject(User user, Project project)
        {
            IEnumerable<HoursRecord> hoursRecords = await GetAllHoursRecordsAsync();
            int totalAssignedHours = hoursRecords.Where(r => (r.User == user) && (r.Project == project)).Sum(h => h.AssignedHours);
            int totalSpentHours = hoursRecords.Where(r => (r.User == user) && (r.Project == project)).Sum(h => h.SpentHours);
            int availableHours = totalAssignedHours - totalSpentHours;
            return  availableHours;
        }

        public async Task AddSpentHoursToUserForProject(HoursRecordForCreation hoursRecord)
        {
            HoursRecord record = new HoursRecord();
            record.User = await userService.FindUser(hoursRecord.UserId);
            record.Project = await projectService.FindProject(hoursRecord.ProjectId);
            record.SpentHours = hoursRecord.Hours;
            db.HoursRecords.Add(record);
            await db.SaveChangesAsync();
        }

       
    }
}
