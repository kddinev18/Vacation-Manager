using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Vacation_Manager.Models;

namespace Vacation_Manager.ViewModel
{
    public static class ProjectLogic
    {
        public static void AddProject(string name, string description)
        {
            try
            {
                Services.AddProject(name, description);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static IEnumerable<ProjectInformation> GetProjects(int userId, int pagingSize, int skipAmount)
        {
            try
            {
                return JsonSerializer.Deserialize<IEnumerable<ProjectInformation>>(Services.GetProjects(userId, pagingSize, skipAmount));
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int GetProjectCount(int userId)
        {
            try
            {
                return Services.GetProjectCount(userId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public static void EditProject(int projectId, string name, string description)
        {
            try
            {
                Services.EditProject(projectId, name, description);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RemoveProject(int projectId)
        {
            try
            {
                Services.RemoveProject(projectId);
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
