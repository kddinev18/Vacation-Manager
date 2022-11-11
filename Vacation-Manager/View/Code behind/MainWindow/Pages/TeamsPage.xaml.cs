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
using Vacation_Manager.View.Code_behind.AddTeam;
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.MainWindow.Pages
{
    /// <summary>
    /// Interaction logic for TeamsPage.xaml
    /// </summary>
    public partial class TeamsPage : Page
    {
        // A collection that updates both ways (form the view and code behind)
        private ObservableCollection<TeamInformation> _teamInformation;
        // The count of the vacations in the database
        private int _teamsCount;
        // The paging size
        private int _pagingSize = 10;
        // The number of pages
        private int _numberOfPages;
        // The page we are on
        private int _pageIndex = 0;
        // The amount of viewed vacations
        private int _sikpAmount = 0;
        public TeamsPage()
        {
            InitializeComponent();
            // Checks if the user is admin, if he isn't disable the AddTeamsButton
            if (!CurrentUserInformation.IsAdmin)
            {
                // Disable the AddTeamsButton
                AddTeamsButton.IsEnabled = false;
            }

            // Get the count of teams
            _teamsCount = TeamLogic.GetTeamsCount(CurrentUserInformation.CurrentUserId.Value);
            // Devide the teams count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_teamsCount / _pagingSize);

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
            // Canges the count of the teams based on the argument i {-1;0;1}
            _teamsCount += i;
            // Devide the vacations count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_teamsCount / _pagingSize);

            // Get the teams from the database
            _teamInformation = new ObservableCollection<TeamInformation>(TeamLogic.GetTeams(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (TeamInformation teamInformation in _teamInformation)
            {
                // Assign the bachground color for the icon of the team
                teamInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                // Assign the inital of the icon of the team
                teamInformation.Initials = teamInformation.Name.Substring(0, 1);

                // Assign the bachground color for the icon of the project
                teamInformation.ProjectBgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                // Assign the inital of the icon of the project
                teamInformation.ProjectInitials = teamInformation.ProjectName.Substring(0, 1);

                // If the user is admin enable the edit button, otherwise disable it
                teamInformation.EditButton = CurrentUserInformation.IsAdmin;
                // If the user is admin enable the remove button, otherwise disable it
                teamInformation.RemoveButton = CurrentUserInformation.IsAdmin;
            }
            // Assign the datagrid the collection
            TeamsDataGrid.ItemsSource = _teamInformation;
        }
        // Event handlers

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
        // Invoked every time the AddTeamsButton is clicked
        private void AddTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            // If the AddTeamWindow isn't opened, oped it, otherwise do nothing
            if (AddTeamWindow.isOpened == false)
            {
                AddTeamWindow addTeamWindow = new AddTeamWindow(this);
                addTeamWindow.Show();
            }
        }

        // Invoked every time the EditButton is clicked
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            TeamInformation dataRow = (TeamInformation)TeamsDataGrid.SelectedItem;
            // Edit a team
            TeamLogic.EditTeam(dataRow.TeamId, dataRow.Name, dataRow.Members);

            // Update the grid
            UpdateDataGrid(0);
        }

        // Invoked every time the RemoveButton is clicked
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            TeamInformation dataRow = (TeamInformation)TeamsDataGrid.SelectedItem;
            // Remove the team
            TeamLogic.RemoveTeam(dataRow.TeamId);

            // Update the grid
            UpdateDataGrid(-1);
        }
    }
}
