/* Author:      Matthew Kotras
 * Co-author:   Stephen Behnke
 * 
 * Description: This class handles all entity framework / database interaction
 *              related to the "User" object and logging, logging out, etc.
 */

using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ScriptBuddy.Logger;

namespace ScriptBuddy.BL
{
    /// <summary>
    /// Implementation of all related code to the User class for the BusinessLayer.
    /// </summary>
    public partial class BusinessLayer : IBusinessLayer
    {
        /// <summary>
        /// A method that allows a user to login to Script Buddy.
        /// </summary>
        /// <param name="username">The username of the user to log in.</param>
        /// <param name="password">The password of the user to log in.</param>
        /// <returns>True if successfully logged in, false otherwise.</returns>
        public User Login(string username, string password)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    User user = null;

                    try
                    {
                        user = context.Users.Where(i => i.Username == username).FirstOrDefault();
                        if (user != null && !BCrypt.Net.BCrypt.Verify(password, user.Password))
                        {
                            user = null;
                        }
                    }
                    catch (Exception e)
                    {
                        // TO-DO: Error Logging
                        throw;
                    }

                    return user;
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// A method that logs the user out.
        /// </summary>
        /// <returns>A null user.</returns>
        public User Logout()
        {
            return null;
        }

        /// <summary>
        /// Gets ALL users from the database.
        /// </summary>
        /// <returns>A list of all the users in the database.</returns>
        public List<User> GetAllUsers()
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    return context.Users.ToList();
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// A method for creating a new user in the database.
        /// </summary>
        /// <param name="username">The new user's username.</param>
        /// <param name="password">The new user's password</param>
        /// <param name="profileName">The new user's profile name.</param>
        /// <returns>A bool that represents if the creation was successful.
        /// and a message related to the creation success,
        /// or errors why it didnt work</returns>
        public (bool success, string message) CreateUser(string username, string password, string profileName)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    // Check if the user already exists with either the same username or profile name.
                    if (context.Users.Where(i => i.Username == username).FirstOrDefault() != null)
                        return (false, "This username has already been taken.");
                    if (context.Users.Where(i => i.ProfileName == profileName).FirstOrDefault() != null)
                        return (false, "This profile name has already been taken.");

                    context.Users.Add(new User()
                    {
                        Id = 0,
                        Username = username,
                        Password = hashedPassword,
                        ProfileName = profileName
                    });

                    return (context.SaveChanges() > 0, "User creation successful.");
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deletes a user and all related information to their account from the database.
        /// </summary>
        /// <param name="username">The username of the user to delete (and all their information / data).</param>
        /// <param name="deleteCommunityScripts">True if the we are to DELETE all community scripts of this user.
        /// False if we are to KEEP all community scripts of this user.</param>
        /// <returns>True if the user was successfully deleted, false otherwise.</returns>
        public bool DeleteUser(string username, bool deleteCommunityScripts = true)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    User user;
                    user = context.Users.Where(i => i.Username.Equals(username)).ToList().FirstOrDefault();

                    if (user != null)
                    {
                        DeleteAllScripts(user.Id, deleteCommunityScripts);

                        context.Remove(user);
                        return context.SaveChanges() > 0;
                    }

                    return true;    // This username does not even exist, we are done.
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// A method for editing a user in the database.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="newUsername">The user's desired username.</param>
        /// <param name="password">The user's password</param>
        /// <param name="newPassword">The user's desired password</param> 
        /// <param name="newProfileName">The user's desired profile name.</param>
        /// <returns>A bool that represents if the update was successful.
        /// and a message related to the update success,
        /// or errors why it didnt work</returns>
        public (bool success, string message) UpdateUser(string username, string newUsername,
            string password, string newPassword, string newProfileName)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    var user = context.Users.Where(i => i.Username.Equals(username)).ToList().FirstOrDefault();

                    try
                    {
                        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                        {
                            return (false, "Password does not match our records.");
                        }
                    }
                    catch (Exception e)
                    {
                        // TO-DO: Error Logging
                        throw;
                    }

                    (bool, string) usernameValidationResult = CredentialUtils.validateUsername(newUsername);
                    (bool, string) passwordValidationResult = CredentialUtils.validatePassword(newPassword);

                    if (!usernameValidationResult.Item1)
                    {
                        return (false, "New username is " + usernameValidationResult.Item2 + ".");
                    }

                    if (newPassword.Length > 0 && !passwordValidationResult.Item1)
                    {
                        return (false, "New password is " + passwordValidationResult.Item2 + ".");
                    }

                    // Check if the user already exists with either the same username or profile name.
                    if (username != newUsername && context.Users.Where(i => i.Username == newUsername).FirstOrDefault() != null)
                        return (false, "This username has already been taken.");
                    if (user.ProfileName != newProfileName && context.Users.Where(i => i.ProfileName == newProfileName).FirstOrDefault() != null)
                        return (false, "This profile name has already been taken.");

                    user.Username = newUsername;
                    user.ProfileName = newProfileName;

                    if (newPassword.Length > 0)
                    {
                        user.Password = hashedPassword;
                    }

                    return (context.SaveChanges() > 0, "User creation successful.");
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Validates the username and password to check if they are for a correct user.
        /// </summary>
        /// <param name="username">The username to be checked.</param>
        /// <param name="password">The password to be checked.</param>
        /// <returns>True if exists and correct, false otherwise.</returns>
        public bool ValidatePassword(string username, string password)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    var user = context.Users.Where(i => i.Username.Equals(username)).ToList().FirstOrDefault();


                    if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }
    }
}
