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

namespace Vacation_Manager.View.Code_behind.UserAuthenticationWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserAuthenticationWindow : Window
    {
        public UserAuthenticationWindow()
        {
            try
            {
                //Services.SetUpConnection();
                InitializeComponent();
            }
            catch (Exception)
            {
                MessageBox.Show("The server is currently down. Plase excuse us.", "Connection error");
                Application.Current.Shutdown();
            }
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
