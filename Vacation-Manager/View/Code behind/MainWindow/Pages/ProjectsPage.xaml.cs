using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vacation_Manager.Models;
using Vacation_Manager.View.Code_behind.AddMember;
using Vacation_Manager.View.Code_behind.AddProject;
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.MainWindow.Pages
{
    /// <summary>
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        // A collection that updates bothways (form the view and code behind)
        private ObservableCollection<ProjectInformation> _projectsInformation;
        // The count of the vacations in the database
        private int _projectCount;
        // The paging size
        private int _pagingSize = 10;
        // The number of pages
        private int _numberOfPages;
        // The page we are on
        private int _pageIndex = 0;
        // The amount of viewed vacations
        private int _sikpAmount = 0;
        public ProjectsPage()
        {
            InitializeComponent();
            // Checks if the user is admin, if he isn't disable the AddProjectsButton
            if (!CurrentUserInformation.IsAdmin)
            {
                // Disable the AddProjectsButton
                AddProjectsButton.IsEnabled = false;
            }

            // Get the count of projects
            _projectCount = ProjectLogic.GetProjectCount(CurrentUserInformation.CurrentUserId.Value);
            // Devide the teams count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_projectCount / _pagingSize);

            // Updates the grid
            UpdateDataGrid(0);

            // Disable the PrevButton
            PrevButton.IsEnabled = false;
            // If the number of pagis is less or equal to 1 disable the NextButton
            if (_numberOfPages <= 1)
            {
                NextButton.IsEnabled = false;
            }
        }
        public void UpdateDataGrid(int i)
        {
            // Canges the count of the projects based on the argument i {-1;0;1}
            _projectCount += i;
            // Devide the vacations count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_projectCount / _pagingSize);
            // Get the projects from the database
            _projectsInformation = new ObservableCollection<ProjectInformation>(ProjectLogic.GetProjects(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (ProjectInformation projectInformation in _projectsInformation)
            {
                // Assign the bachground color for the icon
                projectInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                // Assign the inital of the icon
                projectInformation.Initials = projectInformation.Name.Substring(0, 1);
                // If the user is admin enable the edit button, otherwise disable it
                projectInformation.EditButton = CurrentUserInformation.IsAdmin;
                // If the user is admin enable the remove button, otherwise disable it
                projectInformation.RemoveButton = CurrentUserInformation.IsAdmin;
            }
            // Assign the datagrid the collection
            ProjectDataGrid.ItemsSource = _projectsInformation;
        }
        // Event handlers


        // Invoked every time the AddProjectsButton is clicked
        private void AddProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            // If the AddProjectWindow isn't opened, oped it, otherwise do nothing
            if (!AddProjectWindow.isOpened)
            {
                AddProjectWindow addProjectWindow = new AddProjectWindow(this);
                addProjectWindow.Show();
            }
        }
        // Invoked every time the PrevButton is clicked
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable the NextButton
            NextButton.IsEnabled = true;

            // Decrease the page index
            _pageIndex--;
            // If the page index is 0 diable the PrevButton
            if (_pageIndex == 0)
                PrevButton.IsEnabled = false;

            // Decease the amount of skippings
            _sikpAmount -= _pagingSize;
            // Update the datagrid
            UpdateDataGrid(0);
        }
        // Invoked every time the NextButton is clicked
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable the PrevButton
            PrevButton.IsEnabled = true;

            // Increase the page index 
            _pageIndex++;
            // If the page index is equal to the amount of pages disable NextButton
            if (_pageIndex == _numberOfPages - 1)
                NextButton.IsEnabled = false;

            // Increase the amount of skippings
            _sikpAmount += _pagingSize;
            // Update the datagrid
            UpdateDataGrid(0);
        }

        // Invoked every time the EditButton is clicked
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            ProjectInformation dataRow = (ProjectInformation)ProjectDataGrid.SelectedItem;
            // Edit a project
            ProjectLogic.EditProject(dataRow.ProjectId, dataRow.Name, dataRow.Description);

            // Update the grid
            UpdateDataGrid(0);

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            ProjectInformation dataRow = (ProjectInformation)ProjectDataGrid.SelectedItem;
            // Remove the project
            ProjectLogic.RemoveProject(dataRow.ProjectId);

            // Update the grid
            UpdateDataGrid(-1);
        }
    }
}
