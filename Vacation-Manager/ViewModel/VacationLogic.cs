using ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Vacation_Manager.Models;

namespace Vacation_Manager.ViewModel
{
    public static class VacationLogic
    {
        public static void AddVacation(int userId, DateTime from, DateTime to, string imagePath)
        {
            // Try the code
            try
            {
                // Join the image bytes into a string with separator ';'
                string image = String.Join(';',
                    // Reads all the bytes a file in specific path is made up from
                    File.ReadAllBytes(imagePath)
                );
                Services.AddVacation(userId, from, to , image);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static IEnumerable<VacationInformation> GetVacations(int userId, int pagingSize, int skipAmount)
        {
            // Try the code
            try
            {
                // Get the vacations skipping the already viewed amount the taking only the amount equal to the paging size
                return JsonSerializer.Deserialize<IEnumerable<VacationInformation>>(Services.GetVacations(userId, pagingSize, skipAmount));
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int GetVacationsCount(int userId)
        {
            // Try the code
            try
            {
                // Gets the count of the vacation in the database
                return Services.GetVacationsCount(userId);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public static void ApprooveVacation(int vacationId)
        {
            // Try the code
            try
            {
                // Approve vacation
                Services.ApproveVacation(vacationId);
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
