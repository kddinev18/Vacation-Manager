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
        public bool EditButton { get; set; } = false;
        public bool RemoveButton { get; set; } = false;
        private ObservableCollection<ProjectInformation> _projectsInformation;
        private int _userCount;
        private int _pagingSize = 10;
        private int _numberOfPages;
        private int _pageIndex = 0;
        private int _sikpAmount = 0;
        public ProjectsPage()
        {
            InitializeComponent();
            if (!CurrentUserInformation.IsAdmin)
            {
                AddProjectsButton.IsEnabled = false;
            }

            _userCount = ProjectLogic.GetProjectCount(CurrentUserInformation.CurrentUserId.Value);
            _numberOfPages = (int)Math.Ceiling((double)_userCount / _pagingSize);

            UpdateDataGrid(0);

            PrevButton.IsEnabled = false;
            if (_numberOfPages <= 1)
            {
                NextButton.IsEnabled = false;
            }
        }
        private void AddProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            if(!AddProjectWindow.isOpened)
            {
                AddProjectWindow addProjectWindow = new AddProjectWindow(this);
                addProjectWindow.Show();
            }
        }
        public void UpdateDataGrid(int i)
        {
            _userCount += i;
            _numberOfPages = (int)Math.Ceiling((double)_userCount / _pagingSize);
            _projectsInformation = new ObservableCollection<ProjectInformation>(ProjectLogic.GetProjects(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (ProjectInformation projectInformation in _projectsInformation)
            {
                projectInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                projectInformation.Initials = projectInformation.Name.Substring(0, 1);
                projectInformation.EditButton = CurrentUserInformation.IsAdmin;
                projectInformation.RemoveButton = CurrentUserInformation.IsAdmin;
            }
            ProjectDataGrid.ItemsSource = _projectsInformation;
        }
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;

            _pageIndex--;
            if (_pageIndex == 0)
                PrevButton.IsEnabled = false;

            _sikpAmount -= _pagingSize;
            UpdateDataGrid(0);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PrevButton.IsEnabled = true;

            _pageIndex++;
            if (_pageIndex == _numberOfPages - 1)
                NextButton.IsEnabled = false;

            _sikpAmount += _pagingSize;
            UpdateDataGrid(0);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectInformation dataRow = (ProjectInformation)ProjectDataGrid.SelectedItem;
            ProjectLogic.EditProject(dataRow.ProjectId, dataRow.Name, dataRow.Description);

            UpdateDataGrid(0);

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectInformation dataRow = (ProjectInformation)ProjectDataGrid.SelectedItem;
            ProjectLogic.RemoveProject(dataRow.ProjectId);

            UpdateDataGrid(-1);
        }
    }
}
