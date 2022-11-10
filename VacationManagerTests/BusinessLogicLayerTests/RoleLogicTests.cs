using DataAccessLayer.Data.Model;
using DataAccessLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Logic;

namespace VacationManagerTests.BusinessLogicLayerTests
{
    public class RoleLogicTests
    {
        private VacationManagerContext _vacationManagerContext;
        [TearDown]
        public void TearDown()
        {
            foreach (Role role in _vacationManagerContext.Roles)
            {
                _vacationManagerContext.Roles.Remove(role);
            }

            _vacationManagerContext.SaveChanges();
        }

        [TestCase("TempRole")]
        public void Test_AddRole(string roleIdentificator)
        {
            // Arrange
            _vacationManagerContext = new VacationManagerContext();

            // Act
            RoleLogic.AddRole(roleIdentificator, _vacationManagerContext);

            // Assert
            Assert.That(_vacationManagerContext.Roles != null);
        }
    }
}
