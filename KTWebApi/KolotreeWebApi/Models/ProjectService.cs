using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class ProjectService
    {
        List<Project> projects = new List<Project>
        {
            new Project{ ProjectId=1, Name = "Kolotree", Description = "adadadsdada"},
            new Project{ProjectId = 2, Name="devtech", Description="dsjkkljakdaklj"}
        };

        public void AddProject(Project project)
        {
            if (project != null)
            {
                projects.Add(project);                
            }
        }

        public Project FindProject(int id)
        {
            return projects.FirstOrDefault(p => p.ProjectId == id);
        }

        public void UpdateProject(Project project)
        {
            Project oldProject = projects.FirstOrDefault(p => p.ProjectId == project.ProjectId);
            if (oldProject != null)
            {
                oldProject.Name = project.Name;
                oldProject.Description = project.Description;            
            }           
        }

        public List<Project> FetchAllProjects()
        {
            return projects;
        }
    }
}
