using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KolotreeWebApi.Models;

namespace KolotreeWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/HoursRecord")]
    public class HoursRecordController : Controller
    {
        private readonly HoursRecordService hoursRecordService;
        private readonly UserService userService;
        private readonly ProjectService projectService;

        public HoursRecordController(HoursRecordService _hoursService, UserService _userService, ProjectService _projectService)
        {
            hoursRecordService = _hoursService;
            userService = _userService;
            projectService = _projectService;
        }

        [HttpGet("{id}")]
        private async Task<IActionResult> Get(int id)
        {
            HoursRecord record = await hoursRecordService.GetRacord(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

      
        // POST: api/HoursRecord
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddAssignedHoursToUserForProject([FromBody] HoursRecordForCreation hoursRecord)
        {
            User user = await userService.FindUser(hoursRecord.UserId);
            if (user == null)
            {
                ModelState.AddModelError("User", "The user is not found");
            }
            Project project = await projectService.FindProject(hoursRecord.ProjectId);
            if (project == null)
            {
                ModelState.AddModelError("Project", "The project is not found");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {                              
                HoursRecord record = await hoursRecordService.AddAssignedHoursToUserForProject(hoursRecord);
                return Created($"Get/{record.Id}", record);
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.InnerException.Message);
            }
            
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RemoveAssignedHoursToUserForProject([FromBody] HoursRecordForCreation hoursRecord)
        {
            User user = await userService.FindUser(hoursRecord.UserId);
            if (user == null)
            {
                ModelState.AddModelError("User", "The user is not found");
            }
            Project project = await projectService.FindProject(hoursRecord.ProjectId);
            if (project == null)
            {
                ModelState.AddModelError("Project", "The project is not found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                HoursRecord record = await hoursRecordService.RemoveAssignedHoursToUserForProject(hoursRecord);
                return Created($"Get/{record.Id}", record);
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.InnerException.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddSpentHoursToUserForProject([FromBody] HoursRecordForCreation hoursRecord)
        {
            User user = await userService.FindUser(hoursRecord.UserId);
            if (user == null)
            {
                ModelState.AddModelError("User", "The user is not found");
            }
            Project project = await projectService.FindProject(hoursRecord.ProjectId);
            if (project == null)
            {
                ModelState.AddModelError("Project", "The project is not found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int availableHours = await hoursRecordService.CheckAvailableHoursForUserOnProject(user, project);
            if (availableHours < hoursRecord.Hours)
            {
                ModelState.AddModelError("Hours", "There is no enough available hours for the user on the project. Please add him more assigned hours first.");
            }          
            try
            {
                HoursRecord record = await hoursRecordService.AddSpentHoursToUserForProject(hoursRecord);
                return Created($"Get/{record.Id}", record);
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.InnerException.Message);
            }
        }


       
    }
}
