/* Author: Matthew Kotras
 * 
 * Description: This file contains the logic for the login window.
 */

using ScriptBuddy.BL;
using ScriptBuddy.Models;
using System.Windows;
using System.Windows.Input;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        // Reference to the BL layer
        IBusinessLayer _businessLogic;
        // Reference to the current user
        public User User { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="businessLogic">Reference to the BL layer</param>
        /// <param name="user">Reference to the current user</param>
        public LoginWindow(IBusinessLayer businessLogic, User user)
        {
            InitializeComponent();
            this.User = user;
            _businessLogic = businessLogic;
        }

        /// <summary>
        /// On click of the login button, verify credentials and log in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            (bool, string) usernameValidationResult = CredentialUtils.validateUsername(TextBoxUsername.Text);
            if (!usernameValidationResult.Item1)
            { 
                // Do rudamentary UI checks on the username
                MessageBox.Show("Sorry, your username was " + usernameValidationResult.Item2);
                return;
            }

            User = _businessLogic.Login(TextBoxUsername.Text, PasswordBoxPassword.Password);

            if(User != null)
            {
                MessageBox.Show("Welcome, " + User.ProfileName + "!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Credentials");
            }
        }

        /// <summary>
        /// opens the register window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow(_businessLogic).ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }

        private void MouseDown_DragScreen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }

            }
        }
    }
}
