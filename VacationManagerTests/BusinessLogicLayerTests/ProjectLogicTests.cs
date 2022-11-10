using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data.Model;
using DataAccessLayer;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using BusinessLogicLayer.Logic;
using System.Xml.Linq;

namespace VacationManagerTests.BusinessLogicLayerTests
{
    public class ProjectLogicTests
    {
        private VacationManagerContext _vacationManagerContext;
        [TearDown]
        public void TearDown()
        {
            foreach (Project project in _vacationManagerContext.Projects)
            {
                _vacationManagerContext.Projects.Remove(project);
            }
            foreach (User user in _vacationManagerContext.Users)
            {
                _vacationManagerContext.Users.Remove(user);
            }
            foreach (Role role in _vacationManagerContext.Roles)
            {
                _vacationManagerContext.Roles.Remove(role);
            }
            _vacationManagerContext.SaveChanges();
        }

        [TestCase("TempName", "TempDescription")]
        public void Test_AddProject(string name, string description)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            ProjectLogic.AddProject(name, description, _vacationManagerContext);

            // Assert
            Assert.That(_vacationManagerContext.Projects.Where(project => project.Name == name && project.Description == description) != null);
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword@!1", "TempName", "TempDescription")]
        public void Test_GetProjects(string userName, string email, string password, string name, string description)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            int userId = UserLogic.Register(userName, email, password, _vacationManagerContext);
            ProjectLogic.AddProject(name, description, _vacationManagerContext);

            // Assert
            Assert.That(ProjectLogic.GetProjects(userId, 10, 0, _vacationManagerContext) != null);
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword@!1", "TempName", "TempDescription")]
        public void Test_GetProjectsCount(string userName, string email, string password, string name, string description)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            int userId = UserLogic.Register(userName, email, password, _vacationManagerContext);
            ProjectLogic.AddProject(name, description, _vacationManagerContext);

            // Assert
            Assert.That(ProjectLogic.GetProjectsCount(userId, _vacationManagerContext) == 1);
        }

        [TestCase("TempName", "TempDescription")]
        public void Test_EditProject(string name, string description)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            ProjectLogic.AddProject(name, description, _vacationManagerContext);
            int projectId = _vacationManagerContext.Projects.Where(project => project.Name == name && project.Description == description).First().ProjectId;
            ProjectLogic.EditProject(projectId, "new Name", description, _vacationManagerContext);
            // Assert
            Assert.That(_vacationManagerContext.Projects.Where(project => project.Name == "new Name" && project.Description == description) != null);
        }
    }
}
