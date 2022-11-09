using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Manager.Models;

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
        public static IEnumerable<TeamInformation> GetTeams(int userId, int pagingSize, int skipAmount)
        {
            try
            {
                return JsonSerializer.Deserialize<IEnumerable<TeamInformation>>(Services.GetTeams(userId, pagingSize, skipAmount));
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public static int GetTeamsCount(int userId)
        {
            try
            {
                return Services.GetTeamsCount(userId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
    }
}
