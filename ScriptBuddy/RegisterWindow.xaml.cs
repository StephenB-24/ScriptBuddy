/* Author: Matthew Kotras
 * 
 * Description: This file contains logic for the register account window.
 */

using System.Windows;
using System.Windows.Input;
using ScriptBuddy.BL;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        // TODO: COMMENT ME
        IBusinessLayer _businessLogic;

        /// <summary>
        /// The constructor for RegisterWindow.
        /// </summary>
        public RegisterWindow(IBusinessLayer businessLayer)
        {
            _businessLogic = businessLayer;

            InitializeComponent();
        }

        /// <summary>
        /// Tries to register a new user.
        /// </summary>
        /// <param name="sender">UI element that send the message.</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            (bool, string) usernameValidationResult = CredentialUtils.validateUsername(TextBoxUsername.Text);
            if (!usernameValidationResult.Item1)
            {
                MessageBox.Show("Sorry, your username was " + usernameValidationResult.Item2);
                return;
            }

            if (PasswordBoxPassword.Password != PasswordBoxPasswordConfirm.Password)
            {
                MessageBox.Show("Sorry, your passwords did not match.");
                return;
            }

            (bool, string) passwordValidationResult = CredentialUtils.validatePassword(PasswordBoxPassword.Password);
            if(!passwordValidationResult.Item1)
            {
                MessageBox.Show("Sorry, your password was " + passwordValidationResult.Item2);
                return;
            }

            if (_businessLogic.CreateUser(username: TextBoxUsername.Text, password: PasswordBoxPassword.Password, 
                profileName: TextBoxProfileName.Text).success)
            {
                MessageBox.Show("Successfully created user");
                this.Close();
            }
            else
            {
                MessageBox.Show("Couldn't create username");
            }
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
