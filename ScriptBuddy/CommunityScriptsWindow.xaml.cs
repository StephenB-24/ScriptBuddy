/* Author:      Hailey Vadnais
 * Co-author:   Stephen Behnke
 * 
 * Description: Code behind the CommunityScriptsWindow. This has all the searching and filtering 
 *              functionality that allows people to find and use scripts posted to the community.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScriptBuddy.Logger;
using System.Windows.Input;
using ScriptBuddy.Models;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for CommunityScriptsWindow.xaml.
    /// </summary>
    public partial class CommunityScriptsWindow : Window
    {
        /// <summary>
        /// Reference to the MainWindow this window launched from.
        /// </summary>
        MainWindow mainWindow = null;
        /// <summary>
        /// List of all the public community scripts in the database at window intialization.
        /// </summary>
        List<Script> allCommunityScripts;
        /// <summary>
        /// List of all the relevant public community scripts that match the search query and selected tag.
        /// </summary>
        List<Script> queriedCommunityScripts;
        /// <summary>
        /// List of all the scripts from the queriedCommunityScripts that can fit on the current page.
        /// </summary>
        List<Script> shownCommunityScripts;
        /// <summary>
        /// Current tag the user is filtering on.
        /// </summary>
        CommunityTagsEnum tag;
        /// <summary>
        /// Current search string the user is searching for.
        /// </summary>
        string query;
        /// <summary>
        /// Current page number of queriedCommunityScripts.
        /// </summary>
        int currPageNumber = 1;
        /// <summary>
        /// Total page number based on the number of returned results and the maximum results per page.
        /// </summary>
        int totalPageNumber;
        /// <summary>
        /// Maximum results shown per page.
        /// </summary>
        const int MAX_SCRIPTS_PER_PAGE = 4;

        /// <summary>
        /// Constructor of the Community Scripts Window.
        /// </summary>
        /// <param name="mainWindow">Reference to the Main Window</param>
        public CommunityScriptsWindow(ref MainWindow mainWindow)
        {
            try
            {
                this.mainWindow = mainWindow;

                InitializeComponent();

                tag = CommunityTagsEnum.Tagless;
                query = "";
                allCommunityScripts = this.mainWindow.businessLayer.GetAllCommunityScripts();
                queriedCommunityScripts = allCommunityScripts;

                string[] communityTagsList = Enum.GetNames(typeof(CommunityTagsEnum));
                var comboBoxSelections = from x in communityTagsList select x;
                ComboBoxCommunityTag.ItemsSource = comboBoxSelections;
                totalPageNumber = (int)Math.Ceiling((decimal)allCommunityScripts.Count / MAX_SCRIPTS_PER_PAGE);

                if (allCommunityScripts.Count >= MAX_SCRIPTS_PER_PAGE)
                {
                    shownCommunityScripts = allCommunityScripts.GetRange(0, MAX_SCRIPTS_PER_PAGE);
                    TextBoxResultsNumbers.Text = String.Format("Showing {0} of {1} results", MAX_SCRIPTS_PER_PAGE,
                        allCommunityScripts.Count);
                }
                else
                {
                    shownCommunityScripts = allCommunityScripts.GetRange(0, allCommunityScripts.Count);
                    TextBoxResultsNumbers.Text = String.Format("Showing {0} of {1} results", allCommunityScripts.Count,
                        allCommunityScripts.Count);
                }

                ListViewScriptsView.ItemsSource = shownCommunityScripts;
                ButtonNextPage.Visibility = totalPageNumber == currPageNumber ? Visibility.Hidden : Visibility.Visible;
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
            }
        }

        /// <summary>
        /// Rebinds the ListView when the filtering on scripts changes.
        /// </summary>
        private void Rebind()

        {
            try
            {
                if (ListViewScriptsView != null && queriedCommunityScripts != null)
                {
                    totalPageNumber = (int)Math.Ceiling((decimal)queriedCommunityScripts.Count / MAX_SCRIPTS_PER_PAGE);
                    totalPageNumber = Math.Max(1, totalPageNumber);

                    int min = (currPageNumber - 1) * MAX_SCRIPTS_PER_PAGE;
                    int max = currPageNumber == totalPageNumber ? queriedCommunityScripts.Count % MAX_SCRIPTS_PER_PAGE :
                        MAX_SCRIPTS_PER_PAGE;
                    if (max == 0 && queriedCommunityScripts.Count != 0)
                    {
                        max = MAX_SCRIPTS_PER_PAGE;
                    }

                    TextBoxResultsNumbers.Text = String.Format("Showing {0} of {1} results", max,
                            queriedCommunityScripts.Count);

                    shownCommunityScripts = queriedCommunityScripts.GetRange(min, max);
                    ListViewScriptsView.ItemsSource = shownCommunityScripts;

                    ButtonNextPage.Visibility = totalPageNumber == currPageNumber ? Visibility.Hidden : Visibility.Visible;
                    ButtonPrevPage.Visibility = currPageNumber == 1 ? Visibility.Hidden : Visibility.Visible;
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                MessageBox.Show("An issue occured while rebinding: " + e.ToString());
            }
        }

        /// <summary>+-
        /// When the Insert Into Current button is clicked, the selected script is added into the current working file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonInsertIntoCurrent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the properties of the selected script and load into script sequence
                Script script = (Script)ListViewScriptsView.SelectedItem;

                if (ListViewScriptsView.SelectedItem == null)
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
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("There was an error loading this script: " + ex);
            }
        }

        /// <summary>
        /// When the Open as New Script button is clicked, the selected script is opened in a new file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenAsNewScript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the properties of the selected script and load into script sequence
                Script script = (Script)ListViewScriptsView.SelectedItem;

                if (ListViewScriptsView.SelectedItem == null)
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
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("There was an error loading this script: " + ex);
            }
        }

        /// <summary>
        /// When the TextBox text is changed, filter results on new query string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxScriptSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TextBoxScriptSearch != null && !TextBoxScriptSearch.Text.Equals("Search"))
                {
                    query = TextBoxScriptSearch.Text;

                    UpdateSearchCriteria();
                    currPageNumber = 1;

                    this.Rebind();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// When the ComboBox is closed, filter results on new community tag type selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxCommunityTag_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxCommunityTag.SelectedItem != null)
                {
                    string tagString = (string)ComboBoxCommunityTag.SelectedItem;
                    Enum.TryParse(tagString, out tag);

                    UpdateSearchCriteria();
                    currPageNumber = 1;

                    this.Rebind();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// This updates the search results based on the current user query in the text box 
        /// and the community tag selected
        /// </summary>
        private void UpdateSearchCriteria()
        {
            try
            {
                if (tag == CommunityTagsEnum.Tagless && !TextBoxScriptSearch.Text.Equals("Search"))
                {
                    queriedCommunityScripts = allCommunityScripts.Where(i => i.Name.Contains(query,
                        System.StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                else if (tag == CommunityTagsEnum.Tagless)
                {
                    queriedCommunityScripts = allCommunityScripts.Where(i => i.Name.Contains("")).ToList();
                }
                else
                {
                    queriedCommunityScripts = allCommunityScripts.Where(i => i.Name.Contains(query,
                        System.StringComparison.CurrentCultureIgnoreCase) && i.CommunityTagId == (int)tag).ToList();
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                MessageBox.Show("An error has occurred while updating search criteria: " + e.ToString());
            }
        }

        /// <summary>
        /// When the Next Page button is clicked, set visibility for the Next Page and Prev Page
        /// buttons. Then, show next 1-10 scripts for the current query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNextPage_Click(object sender, RoutedEventArgs e)
        {
            currPageNumber++;

            this.Rebind();
        }

        /// <summary>
        /// When the Next Page button is clicked, set visibility for the Next Page and Prev Page
        /// buttons. Then, show next 1-10 scripts for the current query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPrevPage_Click(object sender, RoutedEventArgs e)
        {
            currPageNumber--;

            this.Rebind();
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