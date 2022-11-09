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
        public bool EditButton { get; set; } = false;
        public bool RemoveButton { get; set; } = false;
        private ObservableCollection<TeamInformation> _teamInformation;
        private int _teamsCount;
        private int _pagingSize = 10;
        private int _numberOfPages;
        private int _pageIndex = 0;
        private int _sikpAmount = 0;
        public TeamsPage()
        {
            InitializeComponent();
            if (!CurrentUserInformation.IsAdmin)
            {
                AddTeamsButton.IsEnabled = false;
            }

            _teamsCount = TeamLogic.GetTeamsCount(CurrentUserInformation.CurrentUserId.Value);
            _numberOfPages = (int)Math.Ceiling((double)_teamsCount / _pagingSize);

            UpdateDataGrid(0);

            PrevButton.IsEnabled = false;
            if (_numberOfPages <= 1)
            {
                NextButton.IsEnabled = false;
            }
        }
        public void UpdateDataGrid(int i)
        {
            _teamsCount += i;
            _numberOfPages = (int)Math.Ceiling((double)_teamsCount / _pagingSize);

            _teamInformation = new ObservableCollection<TeamInformation>(TeamLogic.GetTeams(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (TeamInformation teamInformation in _teamInformation)
            {
                teamInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                teamInformation.Initials = teamInformation.Name.Substring(0, 1);

                teamInformation.ProjectBgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                teamInformation.ProjectInitials = teamInformation.ProjectName.Substring(0, 1);

                teamInformation.EditButton = CurrentUserInformation.IsAdmin;
                teamInformation.RemoveButton = CurrentUserInformation.IsAdmin;
            }
            TeamsDataGrid.ItemsSource = _teamInformation;
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
        private void AddTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddTeamWindow.isOpened == false)
            {
                AddTeamWindow addTeamWindow = new AddTeamWindow(this);
                addTeamWindow.Show();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            TeamInformation dataRow = (TeamInformation)TeamsDataGrid.SelectedItem;
            TeamLogic.EditTeam(dataRow.TeamId, dataRow.Name, dataRow.Members);

            UpdateDataGrid(0);

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            TeamInformation dataRow = (TeamInformation)TeamsDataGrid.SelectedItem;
            TeamLogic.RemoveTeam(dataRow.TeamId);

            UpdateDataGrid(-1);
        }
    }
}
