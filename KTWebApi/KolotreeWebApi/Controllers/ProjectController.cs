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
    [Route("api/Project")]
    public class ProjectController : Controller
    {
        private readonly ProjectService projectService;


        public ProjectController(ProjectService _projectService)
        {
            projectService = _projectService;
        }



        // GET: api/Project
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            return Ok(await projectService.FetchAllProjects());
        }

        // GET: api/Project/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetProject(int id)
        {
            Project project = await projectService.FindProject(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        
        // POST: api/Project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody]Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await projectService.AddProject(project);
                return CreatedAtRoute("Get", new { id = project.ProjectId }, project);
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.Message);
            }
        }
        
        // PUT: api/Project/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody]ProjectForUpdate project)
        {
            Project oldProject = await projectService.FindProject(id);
            if ( oldProject == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await projectService.UpdateProject(project, oldProject);
                return NoContent();
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.Message);
            }
        }

      
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            Project project = await projectService.FindProject(id);
            if ( project == null)
            {
                return NotFound();
            }
            try
            {
                await projectService.DeleteProject(project);
                return NoContent();
            }
            catch (Exception scp)
            {
                return StatusCode(500, scp.Message);
            }
        }
    }
}
