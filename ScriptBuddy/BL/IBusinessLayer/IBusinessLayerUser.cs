/* Author:      Matthew Kotras
 * Co-author:   Stephen Behnke
 * 
 * Description: This interface handles all entity framework / database interaction
 *              related to the "User" object and logging, logging out, etc.
 */

using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptBuddy.BL
{
    /// <summary>
    /// An interface for all methods related to user operations.
    /// </summary>
    public partial interface IBusinessLayer
    {
        /// <summary>
        /// A method that allows a user to login to Script Buddy.
        /// </summary>
        /// <param name="username">The username of the user to log in.</param>
        /// <param name="password">The password of the user to log in.</param>
        /// <returns>True if successfully logged in, false otherwise.</returns>
        public User Login(string username, string password);

        /// <summary>
        /// A method that logs the user out.
        /// </summary>
        /// <returns>A null user.</returns>
        public User Logout();

        /// <summary>
        /// Gets ALL users from the database.
        /// </summary>
        /// <returns>A list of all the users in the database.</returns>
        public List<User> GetAllUsers();

        /// <summary>
        /// A method for creating a new user in the database.
        /// </summary>
        /// <param name="username">The new user's username.</param>
        /// <param name="password">The new user's password</param>
        /// <param name="profileName">The new user's profile name.</param>
        /// <returns>A bool that represents if the creation was successful.
        /// and a message related to the creation success,
        /// or errors why it didnt work</returns>
        public (bool success, string message) CreateUser(string username, string password, string profileName);

        /// <summary>
        /// Deletes a user and all related information to their account from the database.
        /// </summary>
        /// <param name="username">The username of the user to delete (and all their information / data).</param>
        /// <param name="deleteCommunityScripts">True if the we are to DELETE all community scripts of this user.
        /// False if we are to KEEP all community scripts of this user.</param>
        /// <returns>True if the user was successfully deleted, false otherwise.</returns>
        public bool DeleteUser(string username, bool deleteCommunityScripts = true);

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
            string password, string newPassword, string newProfileName);

        /// <summary>
        /// Validates the username and password to check if they are for a correct user.
        /// </summary>
        /// <param name="username">The username to be checked.</param>
        /// <param name="password">The password to be checked.</param>
        /// <returns>True if exists and correct, false otherwise.</returns>
        public bool ValidatePassword(string username, string password);
    }
}
