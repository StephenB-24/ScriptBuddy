/* Author:      Stephen Behnke
 * 
 * Description: This partial main window class is for the properties menu windows. 
 *              When an action is selected from the action sequence, the corresponding properties window
 *              will be loaded and populated.
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
using System.Windows.Documents; using System.Windows.Input; 
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
        // Properties Comboboxes
        /// <summary>
        /// Event handler for the KeyPress properties window when the KeyType combobox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxKeyPressKeyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveCurrentProperties();
        }

        /// <summary>
        /// Event handler for the KeyPress properties window when the PressType combobox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxKeyPressPressType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveCurrentProperties();
        }

        /// <summary>
        /// Event handler for the KeyListener properties window when the KeyType is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxKeyListenerKeyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveCurrentProperties();
        }

        /// <summary>
        /// Event handler for the MouseClick properties window when the ClickButton combobox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxMouseClickButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveCurrentProperties();
        }

        /// <summary>
        /// Event handler for the MouseClick properties window when the PressType combobox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxMouseClickPressType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveCurrentProperties();
        }


        //Textboxes
        /// <summary>
        /// Event handler for the HotString properties window when the OutputString textbox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxHotStringOutputString_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxHotStringOutputString.Text != "")
            {
                SaveCurrentProperties();
            }
        }

        /// <summary>
        /// Event handler for the HotString properties window when the InputString textbox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxHotStringInputString_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxHotStringInputString.Text != "")
            {
                SaveCurrentProperties();
            }
        }

        /// <summary>
        /// Event handler for the MouseMove properties window when the XPosition textbox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxMouseMoveXPosition_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxMouseMoveXPosition.Text != "")
            {
                SaveCurrentProperties();
            }
        }

        /// <summary>
        /// Event handler for the MouseMove properties window when the YPosition textbox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxMouseMoveYPosition_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxMouseMoveYPosition.Text != "")
            {
                SaveCurrentProperties();
            }
        }

        /// <summary>
        /// Event handler for the Pause properties window when the millisecond textbox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxPause_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxPause.Text != "")
            {
                SaveCurrentProperties();
            }
        }

        /// <summary>
        /// Event handler for the CharacterSequence properties window when the character sequence textbox is changed.
        /// It saves it's current properties to whatever the new selection is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCharacterSequence_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveCurrentProperties();
        }


        /// <summary>
        /// This private method saves the current selected attribtues within the properties window
        /// to the respecitve action is selected.
        /// </summary>
        private void SaveCurrentProperties()
        {
            Models.Action selectedAction = (Models.Action)ListBoxActions.SelectedItem;

            if (selectedAction != null && !ignorePropertyChanges)
            {
                int index = ListBoxActions.SelectedIndex;

                if (selectedAction.ActionTypeId == (int)ActionTypeEnum.KeyPress)
                {
                    ((KeyPressProperty)selectedAction.Property).KeyPressed = ComboBoxKeyPressKeyType.SelectedItem.ToString();
                    ((KeyPressProperty)selectedAction.Property).PressType = ComboBoxKeyPressPressType.SelectedItem.ToString();
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.KeyListener)
                {
                    ((KeyListenerProperty)selectedAction.Property).Key = ComboBoxKeyListenerKeyType.SelectedItem.ToString();
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.MouseMove)
                {
                    try
                    {
                        ((MouseMoveProperty)selectedAction.Property).Xposition = int.Parse(TextBoxMouseMoveXPosition.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid X Position");
                    }
                    try
                    {
                        ((MouseMoveProperty)selectedAction.Property).Yposition = int.Parse(TextBoxMouseMoveYPosition.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid Y Position");
                    }
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.MouseClick)
                {
                    ((MouseClickProperty)selectedAction.Property).Button = ComboBoxMouseClickButton.SelectedItem.ToString();
                    ((MouseClickProperty)selectedAction.Property).ClickType = ComboBoxMouseClickClickType.SelectedItem.ToString();
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.Pause)
                {
                    try
                    {
                        ((PauseProperty)selectedAction.Property).PauseDuration = int.Parse(TextBoxPause.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid duration. Please enter valid duration in milliseconds (positive integer).");
                    }
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.CharacterSequence)
                {
                    ((CharacterSequenceProperty)selectedAction.Property).CharacterSequence = TextBoxCharacterSequence.Text;
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.MediaKey)
                {
                    ((MediaKeyProperty)selectedAction.Property).MediaKey = ComboBoxMediaKeyKeyType.SelectedItem.ToString();
                }
                else if (selectedAction.ActionTypeId == (int)ActionTypeEnum.HotString)
                {
                    ((HotStringProperty)selectedAction.Property).InputString = TextBoxHotStringInputString.Text;
                    ((HotStringProperty)selectedAction.Property).OutputString = TextBoxHotStringOutputString.Text;
                }

                RebindListBox();
                ListBoxActions.SelectedIndex = index;
            }
        }
    }
}