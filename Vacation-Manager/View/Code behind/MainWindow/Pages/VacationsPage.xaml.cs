using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Vacation_Manager.View.Code_behind.AddVacation;
using Vacation_Manager.View.Code_behind.ImagePreview;
using Vacation_Manager.ViewModel;

namespace Vacation_Manager.View.Code_behind.MainWindow.Pages
{
    /// <summary>
    /// Interaction logic for VacationsPage.xaml
    /// </summary>
    public partial class VacationsPage : Page
    {
        // A collection that updates bothways (form the view and code behind)
        private ObservableCollection<VacationInformation> _vacationsInformation;
        // The count of the vacations in the database
        private int _vacationsCount;
        // The paging size
        private int _pagingSize = 10;
        // The number of pages
        private int _numberOfPages;
        // The page we are on
        private int _pageIndex = 0;
        // The amount of viewed vacations
        private int _sikpAmount = 0;
        public VacationsPage()
        {
            InitializeComponent();

            // Get the vacations count
            _vacationsCount = VacationLogic.GetVacationsCount(CurrentUserInformation.CurrentUserId.Value);
            // Devide the vacations count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_vacationsCount / _pagingSize);

            // Updates the grid
            UpdateDataGrid(0);

            // Disable the PrevButton
            PrevButton.IsEnabled = false;
            // If the number of pagis is less or equal to 1 disable the NextButton
            if (_numberOfPages <= 1)
            {
                NextButton.IsEnabled = false;
            }
        }
        public void UpdateDataGrid(int i)
        {
            // Canges the count of the vacations based on the argument i {-1;0;1}
            _vacationsCount += i;
            // Devide the vacations count to the paging size to see how many pages are there
            _numberOfPages = (int)Math.Ceiling((double)_vacationsCount / _pagingSize);

            // Get the vacations from the database
            _vacationsInformation = new ObservableCollection<VacationInformation>(VacationLogic.GetVacations(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (VacationInformation vacationInformation in _vacationsInformation)
            {
                // Assign the bachground color for the icon
                vacationInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                // Assign the inital of the icon
                vacationInformation.Initials = vacationInformation.UserName.Substring(0, 1);
                // If the user is admin enable the edit button, otherwise disable it
                vacationInformation.EditButton = CurrentUserInformation.IsAdmin;
            }
            // Assign the datagrid the collection
            VacationDataGrid.ItemsSource = _vacationsInformation;
        }
        // Covert bytes into a image
        private BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imageByteArray))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = memStream;
                img.EndInit();
                img.Freeze();
            }
            return img;
        }
        // Event handlers

        // Invoked every time the PrevButton is clicked
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable the NextButton
            NextButton.IsEnabled = true;

            // Decrease the page index
            _pageIndex--;
            // If the page index is 0 diable the PrevButton
            if (_pageIndex == 0)
                PrevButton.IsEnabled = false;

            // Decease the amount of skippings
            _sikpAmount -= _pagingSize;
            // Update the datagrid
            UpdateDataGrid(0);
        }
        // Invoked every time the NextButton is clicked
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable the PrevButton
            PrevButton.IsEnabled = true;

            // Increase the page index 
            _pageIndex++;
            // If the page index is equal to the amount of pages disable NextButton
            if (_pageIndex == _numberOfPages - 1)
                NextButton.IsEnabled = false;

            // Increase the amount of skippings
            _sikpAmount += _pagingSize;
            // Update the datagrid
            UpdateDataGrid(0);
        }
        // Invoked every time the RequestVacationButton is clicked
        private void RequestVacationButton_Click(object sender, RoutedEventArgs e)
        {
            // If the AddVacationWindow isn't opened, oped it, otherwise do nothing
            if (AddVacationWindow.isOpened == false)
            {
                AddVacationWindow addMemberWindow = new AddVacationWindow(this);
                addMemberWindow.Show();
            }
        }

        // Invoked every time the EditButton is clicked
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the row the user clickd on
            VacationInformation dataRow = (VacationInformation)VacationDataGrid.SelectedItem;
            // Approve the vacation
            VacationLogic.ApprooveVacation(dataRow.VacationId);

            // Update the grid
            UpdateDataGrid(0);
        }
        // Invoked every time the ViewButton is clicked
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            // If the AddVacationWindow isn't opened, oped it, otherwise do nothing
            if (ImagePreviewWindow.isOpened == false)
            {
                // Get the data the user clicked on
                VacationInformation dataRow = (VacationInformation)VacationDataGrid.SelectedItem;

                // Set the image
                ImagePreviewWindow imagePreviewWindow = new ImagePreviewWindow(ConvertByteArrayToBitMapImage(dataRow.Image));
                // Show the window 
                imagePreviewWindow.Show();
            }
        }
    }
}
