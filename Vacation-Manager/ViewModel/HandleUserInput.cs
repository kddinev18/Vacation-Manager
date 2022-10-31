using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiabetesTracker.Logic
{
    public static class HandleUserInput
    {
        // General hander for user input
        public static bool GeneralHandler(string userInput)
        {
            // If the inpt contains ', ' or '|' show error messange boc and return false
            if (string.IsNullOrEmpty(userInput) || userInput.Contains(", ") || userInput.Contains('|'))
            {
                MessageBox.Show("Your input must not contain \', \' or \'|\'", "Error");
                return false;
            }

            // Otherwise return true
            return true;
        }
    }
}
