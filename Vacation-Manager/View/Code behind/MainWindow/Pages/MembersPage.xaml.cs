﻿using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
# nullable disable

namespace Vacation_Manager.View.Code_behind.MainWindow.Pages
{
    /// <summary>
    /// Interaction logic for MembersPage.xaml
    /// </summary>
    public partial class MembersPage : Page
    {
        // A collection that updates both ways (form the view and code behind)
        private ObservableCollection<UserInformation> _usersInformation;
        // The count of the vacations in the database
        private int _userCount;
        // The paging size
        private int _pagingSize = 10;
        // The number of pages
        private int _numberOfPages;
        // The page we are on
        private int _pageIndex = 0;
        // The amount of viewed vacations
        private int _sikpAmount = 0;
        public MembersPage()
        {
            InitializeComponent();
            // Checks if the user is admin, if he isn't disable the AddMembersButton
            if (!CurrentUserInformation.IsAdmin)
            {
                // Disable the AddMembersButton
                AddMembersButton.IsEnabled = false;
            }

            // Get the count of the users without the current user
            _userCount = UserLogic.GetUserCount()-1;
            // Devide the teams count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_userCount / _pagingSize);

            // Updates the grid
            UpdateDataGrid(0);

            // Disable the PrevButton
            PrevButton.IsEnabled = false;
            // If the number of pagis is less or equal to 1 disable the NextButton
            if(_numberOfPages <= 1)
            {
                NextButton.IsEnabled = false;
            }
        }
        public void UpdateDataGrid(int i)
        {
            // Canges the count of the teams based on the argument i {-1;0;1}
            _userCount += i;
            // Devide the vacations count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_userCount / _pagingSize);

            // Get the users from the database
            _usersInformation = new ObservableCollection<UserInformation>(UserLogic.GetUsers(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (UserInformation userInformation in _usersInformation)
            {
                // Assign the bachground color for the icon
                userInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                // Assign the inital of the icon
                userInformation.Initials = userInformation.UserName.Substring(0, 1);
                // If the user is admin enable the edit button, otherwise disable it
                userInformation.EditButton = CurrentUserInformation.IsAdmin;
                // If the user is admin enable the remove button, otherwise disable it
                userInformation.RemoveButton = CurrentUserInformation.IsAdmin;
            }
            // Assign the datagrid the collection
            MemberDataGrid.ItemsSource = _usersInformation;
        }
        public void UpdateDataGrid(string filter)
        {
            // Set the count to 1
            _userCount = 1;
            // Devide the vacations count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_userCount / _pagingSize);

            // Get the users from the database
            UserInformation user = UserLogic.GetUserByName(filter);
            _usersInformation = new ObservableCollection<UserInformation>();
            _usersInformation.Add(user);
            Random r = new Random();
            foreach (UserInformation userInformation in _usersInformation)
            {
                // Assign the bachground color for the icon
                userInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                // Assign the inital of the icon
                userInformation.Initials = userInformation.UserName.Substring(0, 1);
                // If the user is admin enable the edit button, otherwise disable it
                userInformation.EditButton = CurrentUserInformation.IsAdmin;
                // If the user is admin enable the remove button, otherwise disable it
                userInformation.RemoveButton = CurrentUserInformation.IsAdmin;
            }
            // Assign the datagrid the collection
            MemberDataGrid.ItemsSource = _usersInformation;
        }
        // Event handlers
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the datagrid
            UpdateDataGrid(0);
        }

        private void KeyDown_Filter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                UpdateDataGrid(Filter.TextBox.Text);
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
            if(_pageIndex == 0)
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
            if(_pageIndex == _numberOfPages-1)
                NextButton.IsEnabled = false;

            // Increase the amount of skippings
            _sikpAmount += _pagingSize;
            // Update the datagrid
            UpdateDataGrid(0);
        }
        // Invoked every time the AddMembersButton is clicked
        private void AddMembersButton_Click(object sender, RoutedEventArgs e)
        {
            // If the AddMemberWindow isn't opened, oped it, otherwise do nothing
            if (AddMemberWindow.isOpened == false)
            {
                AddMemberWindow addMemberWindow = new AddMemberWindow(this);
                addMemberWindow.Show();
            }
        }

        // Invoked every time the EditButton is clicked
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            UserInformation dataRow = (UserInformation)MemberDataGrid.SelectedItem;
            // Edit a uesr
            UserLogic.EditUser(dataRow.UserId, dataRow.Email, dataRow.RoleIdentificator);

            // Update the grid
            UpdateDataGrid(0);

        }

        // Invoked every time the RemoveButton is clicked
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            UserInformation dataRow = (UserInformation)MemberDataGrid.SelectedItem;
            // Remove the user
            UserLogic.RemoveUser(dataRow.UserId);

            // Update the grid
            UpdateDataGrid(-1);
        }
    }
}
