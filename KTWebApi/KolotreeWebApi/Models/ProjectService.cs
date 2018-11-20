using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ProjectService
    {
        private readonly KolotreeContext db;

        public ProjectService(KolotreeContext _db)
        {
            db = _db;
        }

        public async Task<List<Project>> FetchAllProjects()
        {
            return await db.Projects.ToListAsync();
        }

        public async  Task<Project> FindProject(int id)
        {
            return await db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        }


        public async Task AddProject(Project project)
        {
            db.Projects.Add(project);
            await db.SaveChangesAsync();                   
        }

       

        public async Task UpdateProject(ProjectForUpdate project, Project oldProject)
        {
            oldProject.Name = project.Name;
            oldProject.Description = project.Description;
            await db.SaveChangesAsync();           
        }


        public async Task DeleteProject(Project project)
        {
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            
        }       
    }
}
