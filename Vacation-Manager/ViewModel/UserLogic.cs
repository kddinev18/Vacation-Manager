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
    public static class UserLogic
    {
        public static IEnumerable<UserInformation> GetUsers(int userId, int pagingSize, int skipAmount)
        {
            // Try the code
            try
            {
                // Get the users skipping the already viewed amount the taking only the amount equal to the paging size
                return JsonSerializer.Deserialize<IEnumerable<UserInformation>>(Services.GetUsers(userId, pagingSize, skipAmount));
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int GetUserCount()
        {
            // Try the code
            try
            {
                // Gets the coint of the users in the database
                return Services.GetUserCount();
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public static void RemoveUser(int userId)
        {
            // Try the code
            try
            {
                // Removes a user
                Services.RemoveUser(userId);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void EditUser(int userId, string email, string role)
        {
            // Try the code
            try
            {
                // Edits a user
                Services.EditUser(userId, email, role);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
