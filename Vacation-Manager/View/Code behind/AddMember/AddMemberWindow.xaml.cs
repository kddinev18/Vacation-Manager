using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vacation_Manager.View.Code_behind.MainWindow.Pages;
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.AddMember
{
    /// <summary>
    /// Interaction logic for AddMemberWindow.xaml
    /// </summary>
    public partial class AddMemberWindow : Window
    {
        private bool _isMaximized = false;
        private MembersPage _membersPage;

        public static bool isOpened = false;
        public AddMemberWindow(MembersPage membersPage)
        {
            _membersPage = membersPage;
            InitializeComponent();
            isOpened = true;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Register the user into the database
                UserAuthentocationLogic.RegisterMember(UserName.TextBox.Text, Email.TextBox.Text, PasswordTextBox.Password, Role.TextBox.Text);
                _membersPage.UpdateDataGrid(1);
                isOpened = false;
                this.Close();
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            isOpened = false;
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (_isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 350;
                    this.Height = 350;

                    _isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    _isMaximized = true;
                }
            }
        }
    }
}
