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
            // Try the code
            try
            {
                // Add a project
                Services.AddProject(name, description);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static IEnumerable<ProjectInformation> GetProjects(int userId, int pagingSize, int skipAmount)
        {
            // Try the code
            try
            {
                // Get the projects skipping the already viewed amount the taking only the amount equal to the paging size
                return JsonSerializer.Deserialize<IEnumerable<ProjectInformation>>(Services.GetProjects(userId, pagingSize, skipAmount));
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int GetProjectCount(int userId)
        {
            // Try the code
            try
            {
                // Getting the count of all posts
                return Services.GetProjectCount(userId);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public static void EditProject(int projectId, string name, string description)
        {
            // Try the code
            try
            {
                // Edit a project
                Services.EditProject(projectId, name, description);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RemoveProject(int projectId)
        {
            // Try the code
            try
            {
                // Remove a prject
                Services.RemoveProject(projectId);
            }
            // If there are exception don't crash the application just show a message box
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static ProjectInformation GetProjectByName(string projectName)
        {
            // Try the code
            try
            {
                // Get a prject by name
                return JsonSerializer.Deserialize<ProjectInformation>(Services.GetProjectByName(projectName));
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
