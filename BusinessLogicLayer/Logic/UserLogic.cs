using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataAccessLayer.Data.Model;
using DataAccessLayer;
using System.Collections.ObjectModel;

namespace BusinessLogicLayer.Logic
{
    // POCO class
    public class UserCredentials
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
    }

    public class UserInformation
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleIdentificator { get; set; }
    }
    public static class UserLogic
    {
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
        private static string GetSalt(string userName)
        {
            StringBuilder salt = new StringBuilder();
            Random random = new Random();
            salt.Append(userName.Substring(0, 6));
            for (int i = 0; i < 10; i++)
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

        // Checks if there is a master role> if there isn't, it adds it
        public static void CheckMasterRole(VacationManagerContext dbContext)
        {
            // Search if there is a master role
            foreach (Role existingRoles in dbContext.Roles)
                // If there is stop the function
                if (existingRoles.RoleIdentificator == "Master")
                    throw new AccessViolationException("There is already registered admin in this system. Please contact your administrator");

            // If not create the roleless role
            Role role = new Role()
            {
                RoleIdentificator = "Master"
            };

            // Add the role to the context
            dbContext.Roles.Add(role);

            // Save all changes made in this context into the database
            dbContext.SaveChanges();
        }

        // Creates a new master user
        public static int Register(string userName, string email, string password, VacationManagerContext dbContext)
        {
            // Add master role
            CheckMasterRole(dbContext);

            // Checks if the email is on corrent format
            CheckEmail(email);
            // Checks if the password is on corrent format
            CheckPassword(password);
            // Gets the salt
            string salt = GetSalt(userName);
            // Hashes the password combinded with the salt
            string hashPassword = Hash(password + salt);

            // Add new instance of a User
            User newUser = new User()
            {
                UserName = userName,
                Password = hashPassword,
                Email = email,
                Salt = salt,
            };
            // Assign roleless role
            newUser.Role = dbContext.Roles.Where(role => role.RoleIdentificator == "Master").FirstOrDefault();

            // Add the newly added user into the current context
            dbContext.Users.Add(newUser);

            // Save all changes made in this context into the database
            dbContext.SaveChanges();

            // Returns the newly added user
            return newUser.UserId;
        }

        // Creates a new master user
        public static void RegisterMember(string userName, string email, string password, string roleIdentificator, VacationManagerContext dbContext)
        {
            // Checks if the email is on corrent format
            CheckEmail(email);
            // Checks if the password is on corrent format
            CheckPassword(password);
            // Gets the salt
            string salt = GetSalt(userName);
            // Hashes the password combinded with the salt
            string hashPassword = Hash(password + salt);

            // Add the requested role
            Role role = RoleLogic.AddRole(roleIdentificator, dbContext);

            // Add new instance of a User
            User newUser = new User()
            {
                UserName = userName,
                Password = hashPassword,
                Email = email,
                Salt = salt,
                RoleId = role.RoleId
            };

            // Add the newly added user into the current context
            dbContext.Users.Add(newUser);

            // Save all changes made in this context into the database
            dbContext.SaveChanges();
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

        // Log in with pre-hashed password
        public static int LogInWithPreHashedPassword(string username, string preHashedPassword, VacationManagerContext dbContext)
        {
            List<User> users = dbContext.Users
                // Where the user's username matches the given useraname
                .Where(u => u.UserName == username)
                // Convert the result set to a list
                .ToList();

            // If there are no users with the given user name trow an exception
            if (users.Count == 0)
                throw new ArgumentException("Your password or username is incorrect");

            // For every user check if the pre-Hased password matches
            foreach (User user in users)
            {
                // If pre-Hased passwords matches return the user's id
                if (preHashedPassword == user.Password)
                {
                    return user.UserId;
                }
            }

            // Otherwise throw an exception
            throw new ArgumentException("Your password or username is incorrect");
        }

        // Returns the user information and logs the user in
        public static UserCredentials LogIn(string userName, string password, VacationManagerContext dbContext)
        {
            UserCredentials userCredentials;

            List<User> users = dbContext.Users
                // Where the user's username matches the given useraname
                .Where(u => u.UserName == userName)
                // Convert the result set to a list
                .ToList();

            // If there are no users with the given user name trow an exception
            if (users.Count == 0)
                throw new ArgumentException("Your password or username is incorrect");

            // For every user check is the password matches
            foreach (User user in users)
            {
                // Cheks if the hashed password is equal to the user password
                if (Hash(password + user.Salt.ToString()) == user.Password)
                {
                    // Add new instace of a UserCredentials
                    userCredentials = new UserCredentials()
                    {
                        Id = user.UserId,
                        UserName = user.UserName,
                        HashedPassword = user.Password,
                    };

                    // Returns the newly created UserCredentials
                    return userCredentials;
                }
            }
            // Throws exception if the user couldn't log in
            throw new ArgumentException("Your password or username is incorrect");
        }

        // Checks weather a user has the permissions to perform a specific action
        public static bool CheckAuthorisation(int userId, VacationManagerContext dbContext)
        {
            int roleId = dbContext.Users.Where(user => user.UserId == userId).First().RoleId;
            return dbContext.Roles.Where(role=>role.RoleId == roleId).First().RoleIdentificator == "Master";
        }

        public static IEnumerable<UserInformation> GetUsers(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            // Create a nested context that will be used to retireve role's identificator
            VacationManagerContext nestedDbContext = new VacationManagerContext();
            // Retrieve the next 10 rows of the table users
            IEnumerable<User> users = dbContext.Users.Where(user => user.UserId != userId).Skip(skipAmount).Take(pagingSize);
            ICollection<UserInformation> usersInformation = new List<UserInformation>();
            foreach (User user in users)
            {
                usersInformation.Add(new UserInformation()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleIdentificator = nestedDbContext.Roles.Where(role => role.RoleId == user.RoleId).First().RoleIdentificator
                });
            }
            nestedDbContext.Dispose();
            return usersInformation;
        }

        public static int GetUserCount(VacationManagerContext dbContext)
        {
            return dbContext.Users.Count();
        }

        public static void RemoveUser(int userId, VacationManagerContext dbContext)
        {
            dbContext.Users.Remove(dbContext.Users.Where(user=>user.UserId == userId).First());
            dbContext.SaveChanges();
        }

        public static void EditUser(int userId, string newEmail, string newRoleIdenificator, VacationManagerContext dbContext)
        {
            VacationManagerContext nestedDbContext = new VacationManagerContext();
            User user = dbContext.Users.Where(user => user.UserId == userId).First();
            user.Email = newEmail;
            Role role = nestedDbContext.Roles.Where(role=>role.RoleIdentificator == newRoleIdenificator).FirstOrDefault();
            if(role == null)
            {
                role = new Role()
                {
                    RoleIdentificator = newRoleIdenificator,
                };
                nestedDbContext.Roles.Add(role);
                nestedDbContext.SaveChanges();
            }
            user.Role = role;
            nestedDbContext.Dispose();
            dbContext.SaveChangesAsync();
        }
    }
}
