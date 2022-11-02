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
using Vacation_Manager.View.Code_behind.UserAuthenticationWindow.Pages;

namespace Vacation_Manager.View.Code_behind.UserAuthenticationWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserAuthenticationWindow : Window
    {
        public LogInPage LogInPage { get; set; }
        public RegistrationPage RegistrationPage { get; set; }

        public UserAuthenticationWindow()
        {
            try
            {
                Services.SetUpConnection();
                LogInPage = new LogInPage(this);
                RegistrationPage = new RegistrationPage(this);

                InitializeComponent();
                ShowPage(LogInPage);
            }
            catch (Exception)
            {
                MessageBox.Show("The server is currently down. Please excuse us.", "Connection error");
                Application.Current.Shutdown();
            }
        }
        public void ShowPage(Page page)
        {
            Forms.Content = page;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Drag the window of the button is hold
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the server connection
            Services.RemoveConnection();
            // Shutdown the application
            Application.Current.Shutdown();
        }
    }
}
