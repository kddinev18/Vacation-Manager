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
        public bool EditButton { get; set; } = false;
        public bool RemoveButton { get; set; } = false;
        private ObservableCollection<VacationInformation> _vacationsInformation;
        private int _vacationsCount;
        private int _pagingSize = 10;
        private int _numberOfPages;
        private int _pageIndex = 0;
        private int _sikpAmount = 0;
        public VacationsPage()
        {
            InitializeComponent();

            _vacationsCount = VacationLogic.GetVacationsCount(CurrentUserInformation.CurrentUserId.Value);
            _numberOfPages = (int)Math.Ceiling((double)_vacationsCount / _pagingSize);

            UpdateDataGrid(0);

            PrevButton.IsEnabled = false;
            if (_numberOfPages <= 1)
            {
                NextButton.IsEnabled = false;
            }
        }
        public void UpdateDataGrid(int i)
        {
            _vacationsCount += i;
            _numberOfPages = (int)Math.Ceiling((double)_vacationsCount / _pagingSize);

            _vacationsInformation = new ObservableCollection<VacationInformation>(VacationLogic.GetVacations(CurrentUserInformation.CurrentUserId.Value, _pagingSize, _sikpAmount));
            Random r = new Random();
            foreach (VacationInformation vacationInformation in _vacationsInformation)
            {
                vacationInformation.BgColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                vacationInformation.Initials = vacationInformation.UserName.Substring(0, 1);
                vacationInformation.EditButton = CurrentUserInformation.IsAdmin;
            }
            VacationDataGrid.ItemsSource = _vacationsInformation;
        }
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;

            _pageIndex--;
            if (_pageIndex == 0)
                PrevButton.IsEnabled = false;

            _sikpAmount -= _pagingSize;
            UpdateDataGrid(0);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PrevButton.IsEnabled = true;

            _pageIndex++;
            if (_pageIndex == _numberOfPages - 1)
                NextButton.IsEnabled = false;

            _sikpAmount += _pagingSize;
            UpdateDataGrid(0);
        }
        private void RequestVacationButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddVacationWindow.isOpened == false)
            {
                AddVacationWindow addMemberWindow = new AddVacationWindow(this);
                addMemberWindow.Show();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            VacationInformation dataRow = (VacationInformation)VacationDataGrid.SelectedItem;
            VacationLogic.ApprooveVacation(dataRow.VacationId);

            UpdateDataGrid(0);
        }
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImagePreviewWindow.isOpened == false)
            {
                VacationInformation dataRow = (VacationInformation)VacationDataGrid.SelectedItem;

                ImagePreviewWindow imagePreviewWindow = new ImagePreviewWindow(ConvertByteArrayToBitMapImage(dataRow.Image));
                imagePreviewWindow.Show();
            }
        }
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
    }
}
