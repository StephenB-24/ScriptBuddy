/* Author:      Hailey Vadnais
 * 
 * Description: Code behind the DeleteAccountWindow. This warns the user they are about to delete their
 *              account and checks their password one last time to confirm the delete before removing 
 *              the user's account from the database.
 */

using System.Windows;
using System.Windows.Input;
using ScriptBuddy.BL;
using ScriptBuddy.Models;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for DeleteAccountWindow.xaml.
    /// </summary>
    public partial class DeleteAccountWindow : Window
    {
        /// <summary>
        /// Flag that indicates whether or not the logged-in user has deleted their account or not
        /// </summary>
        public bool Deleted { get; private set; }
        /// <summary>
        /// Window reference to a corresponding IBusinessLayer.
        /// </summary>
        IBusinessLayer businessLayer;
        /// <summary>
        /// Reference to logged-in User.
        /// </summary>
        User user;

        /// <summary>
        /// Constructor for DeleteAccountWindow.
        /// </summary>
        /// <param name="businessLayer"></param>
        /// <param name="user"></param>
        public DeleteAccountWindow(IBusinessLayer businessLayer, User user)
        {
            Deleted = false;
            this.businessLayer = businessLayer;
            this.user = user;

            InitializeComponent();
        }

        /// <summary>
        /// When the user clicks "Delete Account" button, the user account is either deleted if they have
        /// entered the correct password, or they get an error message and the account is not removed from
        /// the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            string username = user.Username;
            string password = PasswordBoxPassword.Password;

            bool deleteCommunityScripts = !(bool)CheckBoxKeepPublic.IsChecked;

            if (businessLayer.ValidatePassword(username, password))
            {
                businessLayer.DeleteUser(username, deleteCommunityScripts);
                Deleted = true;
                MessageBox.Show("Your account has successfully been deleted.");
                this.Close();
            } 
            else
            {
                MessageBox.Show("Account not deleted. Password does not match our records.");
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
