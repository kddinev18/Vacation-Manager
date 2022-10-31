using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Manager.View.Code_behind.UserAuthenticationWindow;
#nullable disable

namespace DiabetesTracker.Logic
{
    public static class UserAuthentocationLogic
    {
        // Add user profile
        public static void FinishRegistration(UserAuthenticationWindow userAuthentication, string country, string city, string about, char gender)
        {
            //Checks the user input
            if (!HandleUserInput.GeneralHandler(country, city, about))
                return;

            try
            {
                // Add user profile
                
                // Open the main window
                
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
