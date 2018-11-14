using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace KolotreeWebApi.Models
{
    public class HoursRecordService
    {
       
        private readonly KolotreeContext db;

        public List<HoursRecord> hoursRecords { get { return db.HoursRecords.ToList(); } private set { } }


        public HoursRecordService(KolotreeContext _db)
        {
            db = _db;
        }
        public bool AddAssignedHoursToUserForProject(HoursRecord hoursRecord, int assignedHours)
        {
            hoursRecord.AssignedHours = assignedHours;
            try
            {
                db.HoursRecords.Add(hoursRecord);
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            
            
        }

        public bool AddSpentHoursToUserForProject(HoursRecord hoursRecord, int spentHours)
        {
            hoursRecord.SpentHours = spentHours;
            try
            {
                db.HoursRecords.Add(hoursRecord);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



    }
}
