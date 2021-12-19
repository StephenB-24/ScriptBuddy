/* Author:      Hailey Vadnais
 * 
 * Description: Code behind the UploadScriptWindow. This window allows the user to give their script a
 *              title, description, and tag before posting it. They also choose here whether they want 
 *              to post the script publicly or privately. Once the form is filled out, this page posts 
 *              the script to the database.
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ScriptBuddy.BL;
using ScriptBuddy.Models;

namespace ScriptBuddy
{  
    /// <summary>
    /// Interaction logic for UploadScriptWindow.xaml.
    /// </summary>
    public partial class UploadScriptWindow : Window
    {
        /// <summary>
        /// Reference to the MainWindow this window launched from.
        /// </summary>
        MainWindow mainWindow = null;


        /// <summary>
        /// Constructor for UploadScriptWindow.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="isCommunity"></param>
        public UploadScriptWindow(ref MainWindow mainWindow, bool isCommunity)
        {
            if (mainWindow.Actions == null || mainWindow.Actions.Count == 0)
            {
                MessageBox.Show("There are no actions in the Main Menu action sequence!\nCome back with a script that is not blank.", "WARNING");
                this.Close();
            }
            this.mainWindow = mainWindow;
            InitializeComponent();

            if (!this.mainWindow.LabelProjectName.Content.Equals(IBusinessLayer.DefaultProjectName))
            {
                TextBoxTitle.Text = this.mainWindow.LabelProjectName.Content.ToString();
            }

            if (isCommunity)
            {
                RadioButtonAccessibilityPublic.IsChecked = true;
            }
            else
            {
                RadioButtonAccessibilityPrivate.IsChecked = true;
            }

            ComboBoxCommunityTag.ItemsSource = Enum.GetNames(typeof(CommunityTagsEnum));
            ComboBoxCommunityTag.SelectedIndex = 0;
        }

        /// <summary>
        /// Checks whether all the fields are filled out correctly when user clicks "Upload" button. If they
        /// have, it submits a request the upload the script to the database. Else, give the user an error
        /// message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxTitle.Text.Length == 0)
                {
                    MessageBox.Show("Title cannot be left empty.");
                }
                else if (TextBoxDescription.Text.Length == 0)
                {
                    MessageBox.Show("Description cannot be left empty.");
                }
                else
                {
                    // See if a script with this name already exists for this user.
                    Script findExistingScript = mainWindow.businessLayer.GetScript(mainWindow.LoggedInUser.Id, TextBoxTitle.Text);

                    if (findExistingScript != null)
                    {
                        MessageBoxResult result = MessageBox.Show("You have a script with this name. Would you like to overwrite it?", "Script Already Exists", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }

                        // If we make it here, this means that the script exists, but is to be overwritten.
                        // First get rid of the old one
                        mainWindow.businessLayer.DeleteScript(findExistingScript.Id);
                    }

                    // Now make the new script and insert
                    Script insertScript = new Script()
                    {
                        UserId = mainWindow.LoggedInUser.Id,
                        Name = TextBoxTitle.Text,
                        Description = TextBoxDescription.Text,
                        CommunityTagId = ComboBoxCommunityTag.SelectedIndex,
                        Accessibility = (bool)RadioButtonAccessibilityPublic.IsChecked,
                        TimeLastSaved = DateTime.Now,
                        Actions = mainWindow.Actions
                    };
                    bool success = mainWindow.businessLayer.InsertScript(insertScript);

                    if (!success)
                    {
                        MessageBox.Show("There was an error inserting this script.", "ERROR");
                    }
                    else
                    {
                        Script tmpScript = mainWindow.businessLayer.GetScript(mainWindow.LoggedInUser.Id, insertScript.Name);
                        this.mainWindow.LabelTimeOfLastSave.Content = insertScript.TimeLastSaved;
                        this.mainWindow.LabelProjectName.Content = insertScript.Name;
                        this.mainWindow.Actions = tmpScript.Actions.ToList();
                        
                        this.mainWindow.RebindListBox();
                        this.mainWindow.RebindPropertyGrid();
                        MessageBox.Show("Script saved successfully!", "SUCCESS");
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error inserting this script: " + ex);
            }
        }

        /// <summary>
        /// Closes the window when the user clicks the "Cancel" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
