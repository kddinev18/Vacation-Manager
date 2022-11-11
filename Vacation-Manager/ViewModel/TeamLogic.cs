using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Manager.Models;
# nullable disable

namespace Vacation_Manager.ViewModel
{
    public static class TeamLogic
    {
        public static void AddTeam(string teamName, string userNames, string projectName)
        {
            // Try the code
            try
            {
                // Add a team
                Services.AddTeam(teamName, userNames, projectName);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static IEnumerable<TeamInformation> GetTeams(int userId, int pagingSize, int skipAmount)
        {
            // Try the code
            try
            {
                // Get the teams skipping the already viewed amount the taking only the amount equal to the paging size
                return JsonSerializer.Deserialize<IEnumerable<TeamInformation>>(Services.GetTeams(userId, pagingSize, skipAmount));
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public static int GetTeamsCount(int userId)
        {
            // Try the code
            try
            {
                // Get the count of teams in the database
                return Services.GetTeamsCount(userId);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
        public static void EditTeam(int teamId, string teamName, string users)
        {
            // Try the code
            try
            {
                // Edits a team
                Services.EditTeam(teamId, teamName, users);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void RemoveTeam(int teamId)
        {
            // Try the code
            try
            {
                // Removes a team
                Services.RemoveTeam(teamId);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static TeamInformation GetTeamByName(string teamName)
        {
            // Try the code
            try
            {
                // Get a team by name
                return JsonSerializer.Deserialize<TeamInformation>(Services.GetTeamByName(teamName));
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
