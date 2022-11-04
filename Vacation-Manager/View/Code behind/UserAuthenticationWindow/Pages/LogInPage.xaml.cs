using ServiceLayer;
using System;
using System.Collections.Generic;
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
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.UserAuthenticationWindow.Pages
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        private UserAuthenticationWindow _userAuthentication;
        public LogInPage(UserAuthenticationWindow userAuthentication)
        {
            _userAuthentication = userAuthentication;
            // Logs with user credentials txt file
            CurrentUserInformation.CurrentUserId = Services.LogInWithCookies();
            // If CurrentUserId is nut null open MainWindow
            if (CurrentUserInformation.CurrentUserId is not null)
            {
                // Open main window
                _userAuthentication.ShowMainWindow();
            }
            InitializeComponent();
        }

        //Event handlers
        private void OpenRegistrationFormButton_Click(object sender, RoutedEventArgs e)
        {
            // Shows RegistrationPage
            _userAuthentication.ShowPage(_userAuthentication.RegistrationPage);
        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            // Log in and sets CurrentUserId to the logged user id
            UserAuthentocationLogic.LogIn(_userAuthentication, UserName.TextBox.Text, PasswordTextBox.Password, RememberMeCheckBox.IsChecked == true ? true : false);
        }
    }
}
