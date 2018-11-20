using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KolotreeWebApi.Models;
using Newtonsoft.Json;

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
        public async Task<IActionResult> GetReportPerUser(int userId, DateTime? fromDate, DateTime? toDate)
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

            ReportPerUser resultReport = await reportService.GetReportPerUser(user, startDate, endDate);      
            return Ok(resultReport);            
        }

        [HttpGet]
        [Route("[action]/{userId}/{projectId}")]
        public async Task<IActionResult> GetReportForUserOnProject(int userId, int projectId)
        {
            User user = await userService.FindUser(userId);
            Project project = await projectService.FindProject(projectId);
            var result = await reportService.GetReportPerUserOnProject(user, project);
            return Ok(project.Name);
        }

    }  
}

