using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Manager.Models;
using Vacation_Manager.View.Code_behind.UserAuthenticationWindow;
#nullable disable

namespace Vacation_Manager.ViewModel
{
    public static class UserAuthentocationLogic
    {
        // Add user profile
        public static void Register(UsersAuthenticationWindow userAuthentication, string userName, string email, string password)
        {
            //Checks the user input
            if (!HandleUserInput.GeneralHandler(userName, email, password))
                return;

            try
            {
                // Register and assign the current user id to the id of the user that has just registered
                CurrentUserInformation.CurrentUserId = Services.Register(userName, email, password);

                // Open the main window
                userAuthentication.ShowMainWindow();

            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public static void LogIn(UsersAuthenticationWindow userAuthentication, string userName, string password, bool doRememberMe)
        {
            //Checks the user input
            if (!HandleUserInput.GeneralHandler(userName, password))
                return;

            try
            {
                // Log in and assign the current user id to the id of the user that has just logged
                CurrentUserInformation.CurrentUserId = Services.LogIn(userName, password, doRememberMe);

                // Open the main window
                userAuthentication.ShowMainWindow();
            }
            catch (Exception exception)
            {
                // Show error message box
                MessageBox.Show(exception.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public static void LogOut()
        {
            Services.RemoveCookies();
        }

        public static void RegisterMember(string userName, string email, string password, string roleIdentificator)
        {
            //Checks the user input
            if (!HandleUserInput.GeneralHandler(userName, email, password, roleIdentificator))
                return;

            // Register the user into the database
            Services.RegisterMember(userName, email, password, roleIdentificator);
        }
    }
}
