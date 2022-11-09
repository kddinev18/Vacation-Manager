using Microsoft.Win32;
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
using System.Windows.Shapes;
using Vacation_Manager.Models;
using Vacation_Manager.View.Code_behind.MainWindow.Pages;
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.AddVacation
{
    /// <summary>
    /// Interaction logic for AddVacationWindow.xaml
    /// </summary>
    public partial class AddVacationWindow : Window
    {
        private bool _isMaximized;
        private VacationsPage _vacationsPage;
        private DateTime _from;
        private DateTime _to;
        private string _selectedImagePath;
        public static bool isOpened = false;
        public AddVacationWindow(VacationsPage vacationsPage)
        {
            InitializeComponent();
            _vacationsPage = vacationsPage;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Register the user into the database
                VacationLogic.AddVacation(CurrentUserInformation.CurrentUserId.Value, _from, _to, _selectedImagePath);
                _vacationsPage.UpdateDataGrid(1);
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
        private void PickAFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Set preview for the selected image
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                _selectedImagePath = dlg.FileName;
                FileHint.Text = _selectedImagePath.Split(@"\").Last();
            }
        }

        private void DatePickerFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;
            _from = picker.SelectedDate.Value;
        }
        private void DatePickerTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;
            _to = picker.SelectedDate.Value;
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
