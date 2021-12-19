/* Author:      Hailey Vadnais
 * 
 * Description: Code behind the AccountSettingsWindow. This allows the user to change their Profile Name,
 *              Username, or Password. This is also where they go to begin the process of deleting their
 *              account, if desired.
 */

using System.Windows;
using ScriptBuddy.BL;
using ScriptBuddy.Models;
using System.Windows.Input;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for AccountSettingsWindow.xaml.
    /// </summary>
    public partial class AccountSettingsWindow : Window
    {
        /// <summary>
        /// Flag that indicates whether or not the logged-in user has deleted their account or not.
        /// </summary>
        public bool Deleted { get; private set; }
        /// <summary>
        /// Logged-in user's Profile Name.
        /// </summary>
        public string ProfileName { get; set; }
        /// <summary>
        /// Logged-in user's Username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Reference to logged-in User.
        /// </summary>
        User user;
        /// <summary>
        /// Window reference to a corresponding IBusinessLayer.
        /// </summary>
        IBusinessLayer businessLayer;

        /// <summary>
        /// Constructor for AccountSettingsWinndow
        /// </summary>
        /// <param name="businessLayer"></param>
        /// <param name="user"></param>
        public AccountSettingsWindow(IBusinessLayer businessLayer, User user)
        {
            this.businessLayer = businessLayer;
            this.user = user;
            this.DataContext = this;
            ProfileName = user.ProfileName;
            Username = user.Username;
            Deleted = false;

            InitializeComponent();
        }

        /// <summary>
        /// Rebinds the pre-filled text boxes in the event that the user changes them
        /// </summary>
        private void Rebind()
        {
            this.ProfileName = user.ProfileName;
            this.Username = user.Username;
            this.TextBoxPassword.Password = "";
            this.TextBoxNewPassword.Password = "";
        }

        /// <summary>
        /// Sends the user to the DeleteAccountWindow when they click the "Delete Account" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            DeleteAccountWindow dac = new DeleteAccountWindow(businessLayer, user);
            dac.ShowDialog();

            if (dac.Deleted)
            {
                Deleted = true;
                this.Close();
            }
        }

        /// <summary>
        /// Saves changes to the user's account based on what they input in the textboxes, as long as all
        /// of the entries are valid and they have entered their correct password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string password = TextBoxPassword.Password;
            string newPassword = TextBoxNewPassword.Password;
            string username = user.Username;
            string newUsername = TextBoxUsername.Text;
            string newProfileName = TextBoxName.Text;

            if (password.Length == 0)
            {
                MessageBox.Show("Please enter your password to make changes.");
            }
            else
            {
                (bool, string) updateUserResult = businessLayer.UpdateUser(username, newUsername,
                    password, newPassword, newProfileName);

                if (updateUserResult.Item1)
                {
                    MessageBox.Show("Profile successfully updated.");
                    user.ProfileName = newProfileName;
                    user.Username = newUsername;
                    Rebind();
                }
                else
                {
                    MessageBox.Show("Profile not updated. " + updateUserResult.Item2);
                }
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
