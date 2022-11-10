using System;
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
using System.Windows.Shapes;
using Vacation_Manager.View.Code_behind.AddMember;
using Vacation_Manager.View.Code_behind.MainWindow.Pages;
using Vacation_Manager.View.Code_behind.UserAuthenticationWindow;
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VacationManagerMainWindow : Window
    {
        private bool _isMaximized = false;
        public Lazy<MembersPage> MembersPage { get; set; }
        public Lazy<DashboardPage> DashboardPage { get; set; }
        public Lazy<ProjectsPage> ProjectsPage { get; set; }
        public Lazy<TeamsPage> TeamsPage { get; set; }
        public Lazy<VacationsPage> VacationsPage { get; set; }
        public VacationManagerMainWindow()
        {
            InitializeComponent();
            MembersPage = new Lazy<MembersPage>();
            DashboardPage = new Lazy<DashboardPage>();
            ProjectsPage = new Lazy<ProjectsPage>();
            TeamsPage = new Lazy<TeamsPage>();
            VacationsPage = new Lazy<VacationsPage>();
            ShowPage(MembersPage.Value);
        }

        public void ShowPage(Page page)
        {
            MainWindowFrame.Content = page;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if(_isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    _isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    _isMaximized = true;
                }
            }
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(DashboardPage.Value);
        }

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(MembersPage.Value);
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(ProjectsPage.Value);
        }

        private void TeamsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(TeamsPage.Value);
        }

        private void VacationsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(VacationsPage.Value);
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            UsersAuthenticationWindow window = new UsersAuthenticationWindow();
            UserAuthentocationLogic.LogOut();
            window.Show();
            this.Close();
        }
    }
}
