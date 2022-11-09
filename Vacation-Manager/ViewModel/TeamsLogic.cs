using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vacation_Manager.ViewModel
{
    public static class TeamsLogic
    {
        public static void AddTeam(string teamName, string userNames, string projectName)
        {
            try
            {
                Services.AddTeam(teamName, userNames, projectName);

            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
