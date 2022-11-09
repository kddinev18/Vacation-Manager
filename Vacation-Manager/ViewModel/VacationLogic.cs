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
            try
            {
                string image = String.Join(';', File.ReadAllBytes(imagePath));
                Services.AddVacation(userId, from, to , image);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static IEnumerable<VacationInformation> GetVacations(int userId, int pagingSize, int skipAmount)
        {
            try
            {
                return JsonSerializer.Deserialize<IEnumerable<VacationInformation>>(Services.GetVacations(userId, pagingSize, skipAmount));
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int GetVacationsCount(int userId)
        {
            try
            {
                return Services.GetVacationsCount(userId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public static void ApprooveVacation(int vacationId)
        {
            try
            {
                Services.ApprooveVacation(vacationId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
