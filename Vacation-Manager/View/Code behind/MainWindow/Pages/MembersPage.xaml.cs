using ServiceLayer;
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
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.MainWindow.Pages
{
    /// <summary>
    /// Interaction logic for MembersPage.xaml
    /// </summary>
    public partial class MembersPage : Page
    {
        private ObservableCollection<UserInformation> _usersInformation;
        private int _userCount;
        private int _pagingSize = 10;
        private int _numberOfPages;
        private int _pageIndex = 0;
        private int _sikpAmount = 0;
        public MembersPage()
        {
            InitializeComponent();

            _userCount = UserLogic.GetUserCount()-1;
            _numberOfPages = (int)Math.Ceiling((double)_userCount / 10);

            UpdateDataGrid();

            PrevButton.IsEnabled = false;
            if(_numberOfPages == 0)
            {
                NextButton.IsEnabled = false;
            }
        }
        private void UpdateDataGrid()
        {
            _usersInformation = new ObservableCollection<UserInformation>(UserLogic.GetUsers(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (UserInformation userInformation in _usersInformation)
            {
                userInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                userInformation.Initials = userInformation.UserName.Substring(0, 1);
            }
            MemberDataGrid.ItemsSource = _usersInformation;
        }
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;

            _pageIndex--;
            if(_pageIndex == 0)
                PrevButton.IsEnabled = false;

            _sikpAmount -= _pagingSize;
            UpdateDataGrid();
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PrevButton.IsEnabled = true;

            _pageIndex++;
            if(_pageIndex == _numberOfPages-1)
                NextButton.IsEnabled = false;

            _sikpAmount += _pagingSize;
            UpdateDataGrid();
        }
        private void AddMembersButton_Click(object sender, RoutedEventArgs e)
        {
            AddMemberWindow addMemberWindow = new AddMemberWindow();
            addMemberWindow.Show();
        }
    }
}
