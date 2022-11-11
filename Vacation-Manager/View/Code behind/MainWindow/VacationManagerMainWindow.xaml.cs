using ServiceLayer;
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
using Vacation_Manager.Models;
using Vacation_Manager.View.Code_behind.AddMember;
using Vacation_Manager.View.Code_behind.MainWindow.Pages;
using Vacation_Manager.View.Code_behind.UserAuthenticationWindow;
using Vacation_Manager.ViewModel;
# nullable disable

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
            UserInformation userInformation = UserLogic.GetCurrrentUserInformation(CurrentUserInformation.CurrentUserId.Value);
            Random r = new Random();
            IconColor.Background = userInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
            IconText.Text = userInformation.UserName.Substring(0,1);
            Username.Text = userInformation.UserName;
            Role.Text = userInformation.RoleIdentificator;
            // Intialise the page as lazy so that they can load when they are requested
            MembersPage = new Lazy<MembersPage>();
            DashboardPage = new Lazy<DashboardPage>();
            ProjectsPage = new Lazy<ProjectsPage>();
            TeamsPage = new Lazy<TeamsPage>();
            VacationsPage = new Lazy<VacationsPage>();
            // Loading the members page intpo the memory and showing it
            ShowPage(MembersPage.Value);
        }
        // Shows a page
        public void ShowPage(Page page)
        {
            MainWindowFrame.Content = page;
        }
        // Event handlers
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the server connection
            Services.RemoveConnection();
            // Shutdown the application
            Application.Current.Shutdown();
        }

        // Invoke every time the user clicks on the window
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Cecks if the button pressed is the left button
            if(e.ChangedButton == MouseButton.Left)
            {
                // Drag the window with the button
                this.DragMove();
            }
        }

        // Invoke every time the user clicks on the window
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Checks if the click count was 2
            if(e.ClickCount == 2)
            {
                // If the window is maximised, minimise it
                if(_isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    _isMaximized = false;
                }
                // Otheewise maximise it
                else
                {
                    this.WindowState = WindowState.Maximized;

                    _isMaximized = true;
                }
            }
        }

        // Invoked every time DashboardButton is clicked
        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(DashboardPage.Value);
        }

        // Invoked every time MembersButton is clicked
        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(MembersPage.Value);
        }

        // Invoked every time ProjectsButton is clicked
        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(ProjectsPage.Value);
        }

        // Invoked every time TeamsButton is clicked
        private void TeamsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(TeamsPage.Value);
        }

        // Invoked every time VacationsButton is clicked
        private void VacationsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(VacationsPage.Value);
        }

        // Invoked every time LogOutButton is clicked
        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            UsersAuthenticationWindow window = new UsersAuthenticationWindow();
            UserAuthentocationLogic.LogOut();
            window.Show();
            this.Close();
        }
    }
}
