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

        // Creates a new user
        public static int Register(string userName, string email, string password)
        {
            // For every existing user check if the email and uername are the same
            foreach (User existingUser in DbContext.Users)
                if (existingUser.Email == email || existingUser.UserName == userName)
                    throw new ArgumentException("There is already a user with that email or username");

            // Checks if the email is on corrent format
            CheckEmail(email);
            // Checks if the password is on corrent format
            CheckPassword(password);
            // Gets the salt
            string salt = GetSalt();
            // Hashes the password combinded with the salt
            string hashPassword = Hash(password + salt);

            // Add new instance of a User
            User newUser = new User()
            {
                UserName = userName,
                Password = hashPassword,
                Email = email,
                Salt = salt
            };

            // Add the newly added user into the current context
            DbContext.Users.Add(newUser);

            // Save all changes made in this context into the database
            DbContext.SaveChanges();

            // Returns the newly added user
            return newUser.UserId;
        }

        // Checks if the email is on corrent format
        private static bool CheckEmail(string email)
        {
            // Check if the email does not constains '@'
            if (email.Contains('@') == false)
                // If the email does not constains '@' it trows axception
                throw new ArgumentException("Email must contain \'@\'");

            // Return true otherwise
            return true;
        }

        // Checks if the password is on corrent format
        private static bool CheckPassword(string pass)
        {
            // Checks if the password is between 10 and 32 characters long
            if (pass.Length <= 10 || pass.Length > 32)
                throw new ArgumentException("Password must be between 10 and 32 charcters");


            // Checks if the password contains a space
            if (pass.Contains(" "))
                throw new ArgumentException("Password must not contain spaces");

            // Checks if the password doesn't conatin upper characters
            if (!pass.Any(char.IsUpper))
                throw new ArgumentException("Password must contain at least 1 upper character");

            // Checks if the password doesn't conatin lower characters
            if (!pass.Any(char.IsLower))
                throw new ArgumentException("Password must contain at least 1 lower character");

            // Checks if the password conatins upper any special symbols
            string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\}]{[_~`+=-" + "\"";
            char[] specialCharactersArray = specialCharacters.ToCharArray();
            foreach (char c in specialCharactersArray)
            {
                if (pass.Contains(c))
                    return true;
            }
            throw new ArgumentException("Password must contain at least 1 special character");
        }

        
    }
}
