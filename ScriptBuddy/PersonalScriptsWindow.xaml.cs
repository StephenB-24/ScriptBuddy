/* Author:      Sebastian Vang
 * Co-author:   Stephen Behnke
 * 
 * Description: 
 */

using ScriptBuddy.BL;
using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents; 
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for PersonalScriptsWindow.xaml
    /// </summary>
    public partial class PersonalScriptsWindow : Window
    {
        // Fields
        /// <summary>
        /// The currently logged in users profile name.
        /// </summary>
        private string ProfileName;
        /// <summary>
        /// Reference to the Main Window.
        /// </summary>
        MainWindow mainWindow = null;


        // Constructor
        /// <summary>
        /// Constructor of the Personal Script Window. Sets labels to the necessary data from the actively 
        /// logged in user.
        /// </summary>
        /// <param name="mainWindow"></param>
        public PersonalScriptsWindow(ref MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.DataContext = this;
            if (mainWindow.LoggedInUser != null)
            {
                this.ProfileName = mainWindow.LoggedInUser.ProfileName;
            }
            else
            {
                this.ProfileName = "Unregistered User";
            }

            InitializeComponent();

            TextBlockProfileName.Text = this.ProfileName;

            RebindListBox();
            RebindActiveScriptInfo();
        }


        // Methods
        /// <summary>
        /// Displays script names in the listbox
        /// </summary>
        private void RebindListBox()
        {
            ScriptListView.ItemsSource = null;
            ScriptListView.ItemsSource = mainWindow.businessLayer.GetAllScripts(mainWindow.LoggedInUser.Id);
        }

        /// <summary>
        /// Binds the actively selected script in the main list to the information labels (i.e. name, num. actions, etc.)
        /// </summary>
        private void RebindActiveScriptInfo()
        {
            TextBlockActiveScriptName.Text = (string)mainWindow.LabelProjectName.Content;
            if (mainWindow.Actions != null)
            {
                TextBlockActiveScriptNumberOfActions.Text = mainWindow.Actions.Count.ToString();
            }
        }

        /// <summary>
        /// Converts a list of Script objects into an array of strings containing the names of the scripts 
        /// </summary>
        /// <param name="scriptList"></param>
        /// <returns>string[] stringArray an array of the names of each script from the given list of Scrip objects</returns>
        private string[] convertToStringArray(List<Script> scriptList)
        {
            string[] stringArray = null;
            for (int i = 0; i < scriptList.Count; i++)
            {
                stringArray[i] = (String)scriptList.ElementAt(i).Name;
            }

            return stringArray;
        }

        /// <summary>
        /// Deletes the selected script from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Delete the selected script 
            Script script = (Script)ScriptListView.SelectedItem;

            if (ScriptListView.SelectedItem == null)
            {
                MessageBox.Show("Please select a script to delete");
            }
            else
            {
                bool result = mainWindow.businessLayer.DeleteScript(script.Id);

                if (result)
                {
                    MessageBox.Show("Successfully deleted script with name: " + script.Name);
                }
                else
                {
                    MessageBox.Show("There was an error deleting the script.");
                }

                this.RebindListBox();
            }

        }

        /// <summary>
        /// Uploads the current script (from the Main Window) to the Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            UploadScriptWindow usw = new UploadScriptWindow(ref this.mainWindow, isCommunity: false);
            try
            {
                usw.ShowDialog();
                this.RebindListBox();
            }
            catch { } // If this is hit, this means that the operation was cancelled because there were no actions in the main menu!
        }

        /// <summary>
        /// Inserts the selected script into the script sequence pane on the Main Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the properties of the selected script and load into script sequence
                Script script = (Script)ScriptListView.SelectedItem;

                if (ScriptListView.SelectedItem == null)
                {
                    MessageBox.Show("Please select a script to load");
                }
                else
                {
                    Script tmpScript = mainWindow.businessLayer.GetScript(script.Id);
                    tmpScript.Actions = tmpScript.Actions.OrderBy(i => i.ActionPosition).ToList();
                    int stop = tmpScript.Actions.Count;
                    int lastId = 1;

                    if (this.mainWindow.ListBoxActions.SelectedItem != null)
                    {
                        lastId = ((Models.Action)(this.mainWindow.ListBoxActions.SelectedItem)).ActionPosition;
                    }
                    else
                    {
                        lastId = this.mainWindow.Actions.Count + 1;
                    }

                    for (int i = lastId - 1; i < this.mainWindow.Actions.Count; i++)
                    {
                        mainWindow.Actions[i].ActionPosition += stop;
                    }

                    for (int i = 0; i < stop; i++, lastId++)
                    {
                        Models.Action newA = tmpScript.Actions.ToList()[i];

                        newA.ActionPosition = lastId;
                        this.mainWindow.Actions.Add(newA);
                    }

                    mainWindow.RebindListBox();
                    mainWindow.RebindPropertyGrid();

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error loading this script: " + ex);
            }
        }

        /// <summary>
        /// Loads the selected script into the script sequence pane on the Main Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the properties of the selected script and load into script sequence
                Script script = (Script)ScriptListView.SelectedItem;

                if (ScriptListView.SelectedItem == null)
                {
                    MessageBox.Show("Please select a script to load");
                }
                else
                {
                    if (this.mainWindow.Actions != null && this.mainWindow.Actions.Count != 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Overwrite the current action sequence in the main menu?", "WARNING", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                    Script tmpScript = mainWindow.businessLayer.GetScript(script.Id);

                    mainWindow.LabelTimeOfLastSave.Content = tmpScript.TimeLastSaved;
                    mainWindow.LabelProjectName.Content = tmpScript.Name;
                    mainWindow.Actions = tmpScript.Actions.ToList();

                    mainWindow.RebindListBox();
                    mainWindow.RebindPropertyGrid();

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error loading this script: " + ex);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MouseDown_DragScreen(object sender, MouseButtonEventArgs e)
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