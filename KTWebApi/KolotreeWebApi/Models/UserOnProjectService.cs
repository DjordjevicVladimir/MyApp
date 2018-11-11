using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolotreeWebApi.Models
{
    public class UserOnProjectService
    {
        private UserService _userService = new UserService();
        private ProjectService _projectService = new ProjectService();
        public  List<UserOnProject> _ListOfUsersOnProjects = new List<UserOnProject>
        {
            new UserOnProject(1, 1, 10),
            new UserOnProject(1, 2, 10),
            new UserOnProject(1, 2, 10),
            new UserOnProject(1, 2, -10),
            new UserOnProject(1, 2, 20),
            new UserOnProject(1, 2, -20),
            new UserOnProject(1, 2, 50),
            new UserOnProject(1, 2, -20),
            new UserOnProject(2, 2, 10),
            new UserOnProject(3, 1, 10),
            new UserOnProject(3, 2, 10),          
        };

        public void AddUserOnProject(UserOnProject userOnProject)
        {
            User user = _userService.FindUser(userOnProject.UserId);
            Project project = _projectService.FindProject(userOnProject.ProjectId);
            if (user == null || project == null)
            {
                return;
            }
            _ListOfUsersOnProjects.Add(userOnProject);
        }

       
    }
}
