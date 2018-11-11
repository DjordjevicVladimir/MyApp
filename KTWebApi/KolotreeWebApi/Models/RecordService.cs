using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class RecordService
    {
        private UserOnProjectService _userOnProjectService = new UserOnProjectService();
        private UserService _userService = new UserService();
        private ProjectService _projectService = new ProjectService();


        private List<Record> MakeRecordListForUserOnProject(List<UserOnProject> userOnProjectList, Project project, User user)
        {            
            List<Record> records = new List<Record>();
            foreach (var rec in userOnProjectList)
            {
                Record r = new Record();
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
                records.Add(r);
            }
            return records;
        }

        private List<Record> MakeRecordListForUser(List<UserOnProject> userOnProjectList,  User user)
        {
            List<Record> records = new List<Record>();
            foreach (var rec in userOnProjectList)
            {
                Record r = new Record();
                r.User = user;
                r.Project = _projectService.FindProject(rec.ProjectId);
                if (rec.Hours > 0)
                {
                    r.AssignedHours = rec.Hours;
                }
                else
                {
                    r.SpentHours = rec.Hours;
                }
                r.DateOfRecord = rec.Date;
                records.Add(r);
            }
            return records;
        }

        private List<Record> MakeRecordListForProject(List<UserOnProject> userOnProjectList, Project project)
        {
            List<Record> records = new List<Record>();
            foreach (var rec in userOnProjectList)
            {
                Record r = new Record();
                r.User = _userService.FindUser(rec.UserId);
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
                records.Add(r);
            }
            return records;
        }

        //<summary>
        //Returns all records for User on Project
        //</summary>
        public List<Record> GetAllRecordsForUserOnProject(Project project, User user)
        {           
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects
                .Where(us => (us.UserId == user.UserId) && (us.ProjectId == project.ProjectId)).ToList();
            List<Record> records = MakeRecordListForUserOnProject(userOnProjectList, project, user);
            return records;
        }

        //<summary>
        //Returns all records for User on Project for given date range
        //</summary>
        public List<Record> GetAllRecordsForUserOnProject(Project project, User user, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return new List<Record>();
            }
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects
                .Where(us => (us.UserId == user.UserId) && (us.ProjectId == project.ProjectId) &&
                (us.Date >= startDate && us.Date <= endDate)).ToList();
            List<Record> records = MakeRecordListForUserOnProject(userOnProjectList, project, user);
            return records;
        }

        //<summary>
        //Returns all records for User
        //</summary>
        public List<Record> GetAllRecordsForUser(User user)
        {
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects
              .Where(u => u.UserId == user.UserId).ToList();

            List<Record> records = MakeRecordListForUser(userOnProjectList, user);
            return records;        

        }

        //<summary>
        //Returns all records for User for given date range
        //</summary>
        public List<Record> GetAllRecordsForUser(User user, DateTime startDate, DateTime endDate)
        {

            if (endDate < startDate)
            {
                return new List<Record>();
            }
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects
              .Where(u => (u.UserId == user.UserId) && (u.Date >= startDate && u.Date <= endDate)).ToList();

            List<Record> records = MakeRecordListForUser(userOnProjectList, user);
            return records;
        }

        //<summary>
        //Returns all records for Project
        //</summary>
        public List<Record> GetAllRecordsForProject(Project project)
        {
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects
              .Where(p => p.ProjectId == project.ProjectId).ToList();

            List<Record> records = MakeRecordListForProject(userOnProjectList, project);
            return records;
        }

        //<summary>
        //Returns all records for Project for given date range
        //</summary>
        public List<Record> GetAllRecordsForProject(Project project, DateTime startDate, DateTime endDate)
        {

            if (startDate > endDate)
            {
                return new List<Record>();
            }
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects
              .Where(p => (p.ProjectId == project.ProjectId) && (p.Date >= startDate && p.Date <= endDate)).ToList();

            List<Record> records = MakeRecordListForProject(userOnProjectList, project);
            return records;
        }

        //<summary>
        //Returns all records 
        //</summary>
        public List<Record> GetAllRecords()
        {
            List<UserOnProject> userOnProjectList = _userOnProjectService._ListOfUsersOnProjects;
            List<Record> records = new List<Record>();
            foreach (var rec in userOnProjectList)
            {
                Record record = new Record();
                record.User = _userService.FindUser(rec.UserId);
                record.Project = _projectService.FindProject(rec.ProjectId);
                if (rec.Hours > 0)
                {
                    record.AssignedHours = rec.Hours;
                }
                else
                {
                    record.SpentHours = rec.Hours;
                }
                record.DateOfRecord = rec.Date;
                records.Add(record);
            }
            return records;
        }


    }
}
