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
    public static class TeamLogic
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
        public static void EditTeam(int teamId, string teamName, string users)
        {
            try
            {
                Services.EditTeam(teamId, teamName, users);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void RemoveTeam(int teamId)
        {
            try
            {
                Services.RemoveTeam(teamId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
