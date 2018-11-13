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

        public bool AddProject(Project project)
        {
            try
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Project FindProject(int id)
        {
            return db.Projects.FirstOrDefault(p => p.ProjectId == id);
        }

        public bool UpdateProject(Project project)
        {
            Project oldProject = db.Projects.FirstOrDefault(p => p.ProjectId == project.ProjectId);
            if (oldProject == null)
            {
                return false;
            }
            oldProject.Name = project.Name;
            oldProject.Description = project.Description;
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Project> FetchAllProjects()
        {
            return db.Projects.ToList();
        }
    }
}
