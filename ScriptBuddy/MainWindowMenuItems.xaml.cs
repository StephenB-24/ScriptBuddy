/* Author:      Stephen Behnke
 * Co-author:   Hailey Vadnais, Matthew Kotras.
 * 
 * Description: This partial class holds Main Window methods pertaining to
 *              the tabs of the Main Menu Window.
 */

using Microsoft.Win32;
using ScriptBuddy.BL;
using ScriptBuddy.Logger;
using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScriptBuddy
{
    /// <summary>
    /// Partial class for the main window containing all methods for the menu bar (toolbar)
    /// </summary>
    public partial class MainWindow : Window
    {
        // File tab
        /// <summary>
        /// File > New
        /// Not implemented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("The current action sequence will be cleared, is this ok?", "Start a new script?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                LabelProjectName.Content = IBusinessLayer.DefaultProjectName;
                LabelTimeOfLastSave.Content = IBusinessLayer.DefaultTimeSaved;

                Actions.Clear();
                RebindListBox();
                RebindPropertyGrid();
            }
        }

        /// File > Export
        /// <summary>
        /// Allows the user to export their script as an AHK file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemExport_Click(object sender, RoutedEventArgs e)
        {
            string code = businessLayerCodeGen.convertActionsToCode(Actions);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AHK(*.ahk)|*.ahk";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, code);
            }
        }

        /// <summary>
        /// This method is called by File>Export.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Edit tab
        /// Edit > Undo
        /// Not implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemUndo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (undoStack.Count == 0)
                {
                    return;
                }
                else
                {
                    redoStack.Push(Actions);
                    Actions = undoStack.Pop();
                    mostRecentActions = Actions;
                    RebindListBox();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occurred while trying to undo: " + ex.ToString());
            }
        }

        /// <summary>
        /// Edit > Redo
        /// Not implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemRedo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (redoStack.Count == 0)
                {
                    return;
                }
                else
                {
                    undoStack.Push(Actions);
                    Actions = redoStack.Pop();
                    mostRecentActions = Actions;
                    RebindListBox();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occurred while trying to redo: " + ex.ToString());
            }
        }

        // Community tab
        /// <summary>
        /// Community > Upload
        /// Opens the upload window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedInUser != null)
                {
                    MainWindow tmpMainWindow = this;
                    UploadScriptWindow usw = new UploadScriptWindow(ref tmpMainWindow, isCommunity: true);
                    try
                    {
                        usw.ShowDialog();
                    }
                    catch { } // If this is hit, this means that the operation was cancelled because there were no actions in the main menu!
                }
                else
                {
                    MessageBox.Show("Please login to upload a script.");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occurred while trying to upload: " + ex.ToString());
            }
        }

        /// <summary>
        /// Opens a community scripts window (modal window).
        /// Called from Community > Browse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow tmpMainWindow = this;
                CommunityScriptsWindow csw = new CommunityScriptsWindow(ref tmpMainWindow);
                csw.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occurred while use the Community Scripts Window: " + ex.ToString());
            }
        }

        // Profile tab
        /// <summary>
        /// View > Profile
        /// Not Implemented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedInUser != null)
                {
                    AccountSettingsWindow asw = new AccountSettingsWindow(businessLayer, LoggedInUser);
                    asw.ShowDialog();

                    if (asw.Deleted)
                    {
                        LoggedInUser = null;
                    }

                    this.RebindUser();
                }
                else
                {
                    MessageBox.Show("Please login to view account settings.");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occurred while use the Profile Window: " + ex.ToString());
            }
        }

        /// <summary>
        /// View > My Scripts
        /// Not Implemented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemViewMyScripts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedInUser != null)
                {
                    MainWindow tmpMainWindow = this;
                    PersonalScriptsWindow psw = new PersonalScriptsWindow(ref tmpMainWindow);
                    psw.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please login to view personal scripts.");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occurred while use the View My Scripts Window: " + ex.ToString());
            }
        }
    }
}