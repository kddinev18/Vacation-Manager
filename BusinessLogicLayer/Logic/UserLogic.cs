using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Vacation_Manager.Data;
using VacationManager;

namespace BusinessLogicLayer.Logic
{
    // POCO class
    public class UserCredentials
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
    }
    public static class UserLogic
    {
        public static VacationManagerDbContext DbContext { get; set; }

        // Retuns the hashed data using the SHA256 algorithm
        private static string Hash(string data)
        {
            // Conver the output to a string and return it
            return BitConverter.ToString
                (
                    // Hashing
                    SHA256.Create().ComputeHash
                    (
                        // convert the string into bytes using UTF8 encoding
                        Encoding.UTF8.GetBytes(data)
                    )
                )
                // Convert all the characters in the string to a uppercase characters
                .ToUpper()
                // Remove the '-' from the hashed data
                .Replace("-", "");
        }
        // Generates a random sequence of characters and numbers
        private static string GetSalt()
        {
            StringBuilder salt = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 16; i++)
            {
                // Add another character into the string builder
                salt.Append
                    (
                        // Converts the output to a char
                        Convert.ToChar
                        (
                            // Generate a random nuber between 0 and 26 and add 65 to it
                            random.Next(0, 26) + 65
                        )
                    );
            }

            // Returns the string builder's string
            return salt.ToString();
        }

        
    }
}
