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

        private bool ValidateStartDate(string fromDate, out DateTime startDate)
        {
            if (fromDate == "")
            {
                startDate = new DateTime(2000, 01, 01);
                return true;
            }
            bool formatCheck = DateTime.TryParse(fromDate, out startDate);
            return formatCheck;
        }

        private bool ValidateEndDate(string toDate, out DateTime endDate)
        {
            if (toDate == "")
            {
                endDate = DateTime.Now.Date;
                return true;
            }
            bool formatCheck = DateTime.TryParse(toDate, out endDate);
            return formatCheck;
        }

       
        private async Task<IActionResult> GetReportForUserOnProject(User user, Project project, string fromDate = "", string toDate = "")
        {
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (project == null)
            {
                return NotFound("Project not found.");
            }
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (fromDateCheck == false || toDateCheck == false)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Ending date of report has to be greater than starting date");
            }
            var result = await reportService.GetReportForUserOnProject(user, project, startDate, endDate);
            return Ok(result);
        }

        private async Task<IActionResult> GetUserReport(User user, string fromDate = "", string toDate = "")
        {
            if (user == null)
            {
                return NotFound("User not found");
            }
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (fromDateCheck == false || toDateCheck == false)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range.");
            }

            UserReport resultReport = await reportService.GetUserReport(user, startDate, endDate);
            return Ok(resultReport);
        }


       


        // GET: api/Report
        [HttpGet]
        [Route("[action]/{userId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetUserReportByUserId(int userId, string fromDate = "", string toDate = "")
        {
            User user = await userService.FindUser(userId);
            return await GetUserReport(user, fromDate, toDate);
        }


        [HttpGet]
        [Route("[action]/{userName}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetUserReportByUserName(string userName, string fromDate = "", string toDate = "")
        {
            User user = await userService.FindUserByUserName(userName);
            return await GetUserReport(user, fromDate, toDate);
        }



        [HttpGet]
        [Route("[action]/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetSumUsersReport(string fromDate = "", string toDate = "")
        {
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (fromDateCheck == false || toDateCheck == false)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range.");
            }

            SumUsersReport resultReport = await reportService.GetSumUsersReport(startDate, endDate);
            return Ok(resultReport);
        }


        [HttpGet]
        [Route("[action]/{userId}/{projectId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetReportForUserOnProjectById(int userId, int projectId, string fromDate= "", string toDate = "")
        {

            User user = await userService.FindUser(userId);            
            Project project = await projectService.FindProject(projectId);
            return await GetReportForUserOnProject(user, project, fromDate, toDate);
        }


        [HttpGet]
        [Route("[action]/{userName}/{projectName}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetReportForUserOnProjectByNames(string userName, string projectName, string fromDate = "", string toDate = "")
        {

            User user = await userService.FindUserByUserName(userName);
            Project project = await projectService.FindProjectByName(projectName);
            return await GetReportForUserOnProject(user, project, fromDate, toDate);
        }

        private async Task<IActionResult> GetUsersSpentHoursReport(User user, string fromDate = "", string toDate = "")
        {
            if (user == null)
            {
                return NotFound("User not found");
            }
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (!fromDateCheck || !toDateCheck)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range.");
            }
            UserSpentHoursReport resultReport = await reportService.GetUsersSpentHoursReport(user, startDate, endDate);
            return Ok(resultReport);

        }

        [HttpGet]
        [Route("[action]/{userId}/{fromDate?}/{toDate?}")]        
        public async Task<IActionResult> GetUsersSpentHoursReportByUserId(int userId, string fromDate= "", string toDate= "")
        {
            User user = await userService.FindUser(userId);
            return await GetUsersSpentHoursReport(user, fromDate, toDate);
        }

        [HttpGet]
        [Route("[action]/{userName}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetUsersSpentHoursReportByUserName(string userName, string fromDate = "", string toDate = "")
        {
            User user = await userService.FindUserByUserName(userName);
            return await GetUsersSpentHoursReport(user, fromDate, toDate);
        }


        private async Task<IActionResult> GetProjectReport(Project project, string fromDate = "", string toDate = "")
        {
            if (project == null)
            {
                return NotFound("Project not found.");
            }
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (fromDateCheck == false || toDateCheck == false)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range.");
            }
            ProjectReport resultReport = await reportService.GetProjectReport(project, startDate, endDate);
            return Ok(resultReport);
        }


        [HttpGet]
        [Route("[action]/{projectId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetProjectReportByProjectId(int projectId, string fromDate = "", string toDate = "")
        {
            Project project = await projectService.FindProject(projectId);
            return await GetProjectReport(project, fromDate, toDate);
        }


        [HttpGet]
        [Route("[action]/{projectName}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetProjectReportByProjectName(string projectName, string fromDate = "", string toDate = "")
        {
            Project project = await projectService.FindProjectByName(projectName);
            return await GetProjectReport(project, fromDate, toDate);
        }


        private async Task<IActionResult> GetProjectSpentHoursReport(Project project, string fromDate = "", string toDate = "")
        {
            if (project == null)
            {
                return NotFound("Project not found.");
            }
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (fromDateCheck == false || toDateCheck == false)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range.");
            }
            ProjectSpentHoursReport resultReport = await reportService.GetProjectSpentHoursReport(project, startDate, endDate);
            return Ok(resultReport);
        }


        [HttpGet]
        [Route("[action]/{projectId}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetProjectSpentHoursReportByProjecId(int projectId, string fromDate = "", string toDate = "")
        {
            Project project = await projectService.FindProject(projectId);
            return await GetProjectSpentHoursReport(project, fromDate, toDate);
        }



        [HttpGet]
        [Route("[action]/{projectName}/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetProjectSpentHoursReportByProjectName(string projectName, string fromDate = "", string toDate = "")
        {
            Project project = await projectService.FindProjectByName(projectName);
            return await GetProjectSpentHoursReport(project, fromDate, toDate);
        }



        [HttpGet]
        [Route("[action]/{fromDate?}/{toDate?}")]
        public async Task<IActionResult> GetSumProjectsReport(string fromDate = "", string toDate = "")
        {
            DateTime startDate;
            bool fromDateCheck = ValidateStartDate(fromDate, out startDate);
            DateTime endDate;
            bool toDateCheck = ValidateEndDate(toDate, out endDate);
            if (fromDateCheck == false || toDateCheck == false)
            {
                return BadRequest("Invalid date format. Acceptable formats are yyyy-MM-dd and dd-MM-yyyy.");
            }
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range.");
            }

            SumProjectsReport resultReport = await reportService.GetSumProjectsReport(startDate, endDate);
            return Ok(resultReport);
        }

       

    }  
}

