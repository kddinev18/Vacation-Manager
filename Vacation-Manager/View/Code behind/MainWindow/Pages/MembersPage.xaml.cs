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
using Vacation_Manager.View.Code_behind.AddMember;

namespace Vacation_Manager.View.Code_behind.MainWindow.Pages
{
    /// <summary>
    /// Interaction logic for MembersPage.xaml
    /// </summary>
    public partial class MembersPage : Page
    {
        public MembersPage()
        {
            InitializeComponent();
        }

        private void AddMembersButton_Click(object sender, RoutedEventArgs e)
        {
            AddMemberWindow addMemberWindow = new AddMemberWindow();
            addMemberWindow.Show();
        }
    }
}
