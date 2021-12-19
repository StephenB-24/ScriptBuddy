/* Author:      Stephen Behnke
 * 
 * Description: This window is used for adding a new action to the action sequence
 *              back at the Main Window.
 */

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
using System.Windows.Shapes;
using ScriptBuddy.Models;
using ScriptBuddy.BL;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for AddActionWindow.xaml
    /// </summary>
    public partial class AddActionWindow : Window
    {
        /// <summary>
        /// The active business layer for the program.
        /// </summary>
        IBusinessLayer businessLayer;

        /// <summary>
        /// The actionType that is selected to be added to the Main Window.
        /// </summary>
        public int ActionType { get; private set; }

        /// <summary>
        /// Constructor for the Add Action Window. Initializes the actions listbox.
        /// </summary>
        /// <param name="businessLayer">The business layer used for the program</param>
        public AddActionWindow(IBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
            InitializeComponent();

            ListBoxActionsAvailable.ItemsSource = Enum.GetNames(typeof(ActionTypeEnum));
        }

        /// <summary>
        /// Triggered when the button to add the action is pressed. Closes the window automatically.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddAction_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxActionsAvailable.SelectedItem != null)
            {
                ActionType = ListBoxActionsAvailable.SelectedIndex + 1;
                this.Close();
            }
            else
            {
                MessageBox.Show("No item selected.");
            }
        }

        /// <summary>
        /// Closes the Add Action Window.
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