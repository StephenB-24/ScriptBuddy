/**
 * Author: Matthew Kotras
 */

namespace ScriptBuddy
{
    /// <summary>
    /// Set of useful methods for managing user credentials.
    /// </summary>
    public static class CredentialUtils
    {
        /// <summary>
        /// Validates a username.
        /// </summary>
        /// <param name="password">The username to be checked.</param>
        /// <returns>a boolean whether or not it was valid, 
        /// and if it was not valid, a string containing the reason in form
        /// "Sorry, your username was _______".</returns>
        public static (bool valid, string message) validateUsername(string username)
        {
            if (username.Length == 0)
            {
                return (false, "too short");
            }

            if (username.Length > 255)
            {
                return (false, "too long");
            }
            return (true, "");
        }
        
        /// <summary>
        /// Validates a password.
        /// </summary>
        /// <param name="password">The password to be checked.</param>
        /// <returns>a boolean whether or not it was valid, 
        /// and if it was not valid, a string containing the reason in form
        /// "Sorry, your password was _______".</returns>
        public static (bool valid, string message) validatePassword(string password)
        {
            if(password.Length < 8)
            {
                return (false, "too short");
            }

            if(password.Length > 255)
            {
                return (false, "too long");
            }
            return (true, "");
        }
    }
}
