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
/* Author: Matthew Kotras
 * 
 * Description: This file contains all the logic pertaining to the confirmation window when you close the program.
 */

namespace ScriptBuddy
{
    /// <summary>
    /// The window for when you close the program but still have something running.
    /// </summary>
    public partial class ClosingConfirmationWindow : Window
    {
        public bool cancel { get; private set; }
        public ClosingConfirmationWindow()
        {
            InitializeComponent();
            cancel = true;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.cancel = false;
            this.Close();
        }

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
