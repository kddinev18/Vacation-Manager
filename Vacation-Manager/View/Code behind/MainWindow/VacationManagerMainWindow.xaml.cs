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

namespace Vacation_Manager.View.Code_behind.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VacationManagerMainWindow : Window
    {
        private bool _isMaximized = false;
        public MembersPage MembersPage { get; set; }
        public DashboardPage DashboardPage { get; set; }
        public ProjectsPage ProjectsPage { get; set; }
        public TeamsPage TeamsPage { get; set; }
        public VacationManagerMainWindow()
        {
            InitializeComponent();
            MembersPage = new MembersPage();
            DashboardPage = new DashboardPage();
            ProjectsPage = new ProjectsPage();
            TeamsPage = new TeamsPage();
            ShowPage(MembersPage);
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
            ShowPage(DashboardPage);
        }

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(MembersPage);
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(ProjectsPage);
        }

        private void TeamsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(TeamsPage);
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
