using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KolotreeWebApi.Models;
using Newtonsoft.Json;
using KolotreeWebApi.Models.Reports;


namespace KolotreeWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {

        private readonly ReportService reportService;
        private readonly UserService userService;
        private readonly ProjectService projectService;

        public ReportController(ReportService _reportService, UserService _userService, ProjectService _projectService)
        {
            reportService = _reportService;
            userService = _userService;
            projectService = _projectService;
        }



        // GET: api/Report
        [HttpGet]
        [Route("[action]/{userId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetUserReport(int userId, DateTime? fromDate, DateTime? toDate)
        {
            User user = await userService.FindUser(userId);
            if (user == null)
            {
                return NotFound("Not found user with given ID.");
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }

            UserReport resultReport = await reportService.GetUserReport(user, startDate, endDate);      
            return Ok(resultReport);            
        }


        [HttpGet]
        [Route("[action]/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetSumUsersReport(DateTime? fromDate, DateTime? toDate)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }

            SumUsersReport resultReport = await reportService.GetSumUsersReport(startDate, endDate);
            return Ok(resultReport);
        }


        [HttpGet]
        [Route("[action]/{userId}/{projectId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetReportForUserOnProject(int userId, int projectId, DateTime? fromDate, DateTime? toDate)
        {

            User user = await userService.FindUser(userId);
            if (user == null)
            {
                return NotFound("Not found user with given ID.");
            }
            Project project = await projectService.FindProject(projectId);
            if (project == null)
            {
                return NotFound("Not found project with given id.");
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }
            var result = await reportService.GetReportForUserOnProject(user, project, startDate, endDate );
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{userId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetUsersSpentHoursReport(int userId, DateTime? fromDate, DateTime? toDate)
        {
            User user = await userService.FindUser(userId);
            if (user == null)
            {
                return NotFound("Not found user with given ID.");
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }
            UserSpentHoursReport resultReport = await reportService.GetUsersSpentHoursReport(user, startDate, endDate);
            return Ok(resultReport);
        }

        [HttpGet]
        [Route("[action]/{projectId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetProjectReport(int projectId, DateTime? fromDate, DateTime? toDate)
        {
            Project project = await projectService.FindProject(projectId);
            if (project == null)
            {
                return NotFound("Not found project with given id.");
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }
            ProjectReport resultReport = await reportService.GetProjectReport(project, startDate, endDate);
            return Ok(resultReport);
        }


        [HttpGet]
        [Route("[action]/{projectId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetProjectSpentHoursReport(int projectId, DateTime? fromDate, DateTime? toDate)
        {
            Project project = await projectService.FindProject(projectId);
            if (project == null)
            {
                return NotFound("Not found project with given id.");
            }
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }
            ProjectSpentHoursReport resultReport = await reportService.GetProjectSpentHoursReport(project, startDate, endDate);
            return Ok(resultReport);
        }

        [HttpGet]
        [Route("[action]/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetSumProjectsReport(DateTime? fromDate, DateTime? toDate)
        {
            DateTime startDate = fromDate ?? new DateTime(2000, 01, 01);
            DateTime endDate = toDate ?? DateTime.Now;
            if (startDate > endDate)
            {
                return NotFound("Ending date of report has to be greater than starting date");
            }

            SumProjectsReport resultReport = await reportService.GetSumProjectsReport(startDate, endDate);
            return Ok(resultReport);
        }

       

    }  
}

