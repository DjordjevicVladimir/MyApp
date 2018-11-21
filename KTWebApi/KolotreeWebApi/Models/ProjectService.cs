using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ProjectService
    {
        private readonly KolotreeDbContext db;

        public ProjectService(KolotreeDbContext _db)
        {
            db = _db;
        }


        public async Task<List<Project>> FetchAllProjects()
        {
            return await db.Projects.ToListAsync();
        }

        public async Task<Project> FindProject(int id)
        {
            return await db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        }


        public async Task<Project> AddProject(ProjectForManipulation project)
        {
            Project newProject = new Project { Name = project.Name, Description = project.Description };
            db.Projects.Add(newProject);
            await db.SaveChangesAsync();
            return newProject;
        }



        public async Task UpdateProject(ProjectForManipulation project, Project oldProject)
        {
            oldProject.Name = project.Name;
            oldProject.Description = project.Description;
            await db.SaveChangesAsync();
        }


        public async Task DeleteProject(Project project)
        {
            //db.Projects.Remove(project);
            //await db.SaveChangesAsync();

        }
    }
}
