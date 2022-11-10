using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Logic;
using DataAccessLayer;
using DataAccessLayer.Data.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace VacationManagerTests.BusinessLogicLayerTests
{
    public class UserLogicTests
    {
        private VacationManagerContext _vacationManagerContext;
        [TearDown]
        public void TearDown()
        {
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

        [TestCase("TempUserName","TempEmail@abv.bg","TempPassword!@1","TempRole")]
        public void Test_RegisterMember(string userName, string email, string password, string roleIdentificator)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            UserLogic.RegisterMember(userName, email, password, roleIdentificator, _vacationManagerContext);

            // Assert
            Assert.That(_vacationManagerContext.Users.Where(user=>user.UserName == userName && user.Email == email) != null);
        }

        [TestCase("TempUserName", "TempEmail", "TempPassword!@1", "TempRole")]
        public void Test_RegisterMember_WrongEmail(string userName, string email, string password, string roleIdentificator)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Assert
            Assert.Throws(new ArgumentException().GetType(), () => UserLogic.RegisterMember(userName, email, password, roleIdentificator, _vacationManagerContext));
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "temppassword", "TempRole")]
        public void Test_RegisterMember_WrongPassword(string userName, string email, string password, string roleIdentificator)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Assert
            Assert.Throws(new ArgumentException().GetType(), () => UserLogic.RegisterMember(userName, email, password, roleIdentificator, _vacationManagerContext));
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword@!1")]
        public void Test_Register(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            UserLogic.Register(userName, email, password, _vacationManagerContext);

            // Assert
            Assert.That(_vacationManagerContext.Users.Where(user=>user.UserName == userName && user.Email == email) != null);
        }

        [TestCase("TempUserName", "TempEmail", "TempPassword!@1")]
        public void Test_Register_WrongEmail(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Assert
            Assert.Throws(new ArgumentException().GetType(), () => UserLogic.Register(userName, email, password, _vacationManagerContext));
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "temppassword")]
        public void Test_Register_WrongPassword(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Assert
            Assert.Throws(new ArgumentException().GetType(), () => UserLogic.Register(userName, email, password, _vacationManagerContext));
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_CheckMasterRole(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();
            UserLogic.Register(userName, email, password, _vacationManagerContext);


            // Assert
            Assert.Throws(new AccessViolationException().GetType(), () => UserLogic.CheckMasterRole(_vacationManagerContext));
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_LogIn(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();
            UserLogic.Register(userName, email, password, _vacationManagerContext);

            UserCredentials userCredentials = UserLogic.LogIn(userName,password,_vacationManagerContext);

            // Assert
            Assert.That(userCredentials != null);
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_LogIn_WrongCredentials(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();
            UserLogic.Register(userName, email, password, _vacationManagerContext);

            // Assert
            Assert.Throws(new ArgumentException().GetType(), () => UserLogic.LogIn(userName + "WrongUsername", password, _vacationManagerContext));

        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_GetUsers(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            int userId = UserLogic.Register(userName, email, password, _vacationManagerContext);

            // Assert
            Assert.That(UserLogic.GetUsers(userId,10,0, _vacationManagerContext) != null);
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_GetUsersCount(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();
            UserLogic.Register(userName, email, password, _vacationManagerContext);
        
            // Assert
            Assert.That(UserLogic.GetUserCount(_vacationManagerContext) == 1);
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_RemoveUser(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();
            int userId = UserLogic.Register(userName, email, password, _vacationManagerContext);

            // Act
            UserLogic.RemoveUser(userId, _vacationManagerContext);

            // Assert
            Assert.That(UserLogic.GetUserCount(_vacationManagerContext) == 0);
        }

        [TestCase("TempUserName", "TempEmail@abv.bg", "TempPassword!@1")]
        public void Test_EditUser(string userName, string email, string password)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();
            int userId = UserLogic.Register(userName, email, password, _vacationManagerContext);

            // Act
            UserLogic.EditUser(userId, "TempEmail22@abv.bg", "Master", _vacationManagerContext);

            // Assert
            Assert.That(_vacationManagerContext.Users.Where(user=>user.Email== "TempEmail22@abv.bg")!=null);
        }
    }
}
