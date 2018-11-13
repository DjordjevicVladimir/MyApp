using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class HoursRecordService
    {
        private UserService _userService = new UserService();
        private ProjectService _projectService = new ProjectService();
        public  List<HoursRecord> recordsList = new List<HoursRecord>
        {
            new HoursRecord(1, 1, 10, 0 ),
            new HoursRecord(1, 2, 10, 0),
            new HoursRecord(1, 2, 10, 0 ),
            new HoursRecord(1, 2, 0, 10),
            new HoursRecord(1, 2, 20, 0),
            new HoursRecord(1, 2, 0, 20),
            new HoursRecord(1, 2, 50, 0 ),
            new HoursRecord(1, 2, 0, 20),
            new HoursRecord(2, 2, 10, 0 ),
            new HoursRecord(3, 1, 10, 0),
            new HoursRecord(3, 2, 10, 0),          
        };

        public void AddAssignedHoursToUserForProject(int userId, int projectId, int assignedHours)
        {
            User user = _userService.FindUser(userId);
            Project project = _projectService.FindProject(userId);
            if (user == null || project == null)
            {
                return;
            }
            HoursRecord hoursRecord = new HoursRecord(userId, projectId);
            hoursRecord.AssignedHours = assignedHours;

            recordsList.Add(hoursRecord);
        }

        public void AddSpentHoursToUserForProject(int userId, int projectId, int spentHours)
        {
            User user = _userService.FindUser(userId);
            Project project = _projectService.FindProject(userId);
            if (user == null || project == null)
            {
                return;
            }
            HoursRecord hoursRecord = new HoursRecord(userId, projectId);
            hoursRecord.SpentHours = spentHours;

            recordsList.Add(hoursRecord);
        }



    }
}
