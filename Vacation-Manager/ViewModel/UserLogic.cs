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
        public static ICollection<UserInformation> GetUsers(int userId, int pagingSize, int skipAmount)
        {
            try
            {
                // Converts the text into UserInformation objects and returns it
                return JsonSerializer.Deserialize<ICollection<UserInformation>>(Services.GetUsers(userId, pagingSize, skipAmount));
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int GetUserCount()
        {
            try
            {
                return Services.GetUserCount();
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public static void RemoveUser(int userId)
        {
            try
            {
                Services.RemoveUser(userId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
