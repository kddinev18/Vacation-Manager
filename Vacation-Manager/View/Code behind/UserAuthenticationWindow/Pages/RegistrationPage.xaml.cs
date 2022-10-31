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

namespace Vacation_Manager.View.Code_behind.UserAuthenticationWindow.Pages
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private UserAuthenticationWindow _userAuthentication;
        public RegistrationPage(UserAuthenticationWindow userAuthentication)
        {
            _userAuthentication = userAuthentication;
            InitializeComponent();
        }

        private void OpenLogInFormButton_Click(object sender, RoutedEventArgs e)
        {
            // Show LogInPage
            _userAuthentication.ShowPage(_userAuthentication.LogInPage);
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Register and set the CurrentUserId
        }
    }
}
