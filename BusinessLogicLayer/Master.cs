using BusinessLogicLayer.Logic;
using System;
using Vacation_Manager.Data;

namespace BusinessLogicLayer
{
    public class Master
    {
        public void OpenConnection()
        {
            VacationManagerDbContext vacationManagerDbContext = new VacationManagerDbContext();
            vacationManagerDbContext.ChangeTracker.Clear();
            //UserLogic.DbContext = vacationManagerDbContext;
        }

        public void CloseConnection()
        {
            //UserLogic.DbContext = null;
        }
    }
}
