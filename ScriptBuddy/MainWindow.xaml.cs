/* Author:      Stephen Behnke
 * Co-author:   Sebastian Vang, Matthew Kotras.
 * 
 * Description: This partial class holds Main Window methods pertaining to the buttons
 *              on the Main Window as well as some of the basic window functionalities (dragging).
 */

using ScriptBuddy.BL;
using ScriptBuddy.Logger;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScriptBuddy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Fields
        /// <summary>
        /// A list that holds the most recent user actions.
        /// </summary>
        private List<Models.Action> mostRecentActions = new List<Models.Action>();
        /// <summary>
        /// A stack used for undo operations.
        /// </summary>
        private Stack<List<Models.Action>> undoStack = new Stack<List<Models.Action>>();
        /// <summary>
        /// A stack used for the redo operations.
        /// </summary>
        private Stack<List<Models.Action>> redoStack = new Stack<List<Models.Action>>();
        /// <summary>
        /// A boolean used to temporarily turn off event handlers while data is being manipulated.
        /// </summary>
        private bool ignorePropertyChanges = false;


        // Properties
        /// <summary>
        /// The business layer reference that will be used for the life of the program.
        /// </summary>
        public IBusinessLayer businessLayer { get; set; }
        /// <summary>
        /// The business layer codegen (used for code generation) that will be used for the life of the program.
        /// </summary>
        public BL.CodeGen.BusinessLayerCodeGen businessLayerCodeGen { get; set; }
        /// <summary>
        /// The currently logged in user.
        /// </summary>
        public User LoggedInUser { get; set; }
        /// <summary>
        /// The current action sequence list.
        /// </summary>
        public List<Models.Action> Actions { get; set; }


        /// <summary>
        /// Constructor for the Main Window, the heart of the program.
        /// </summary>
        public MainWindow()
        {
            // Splash Screen.
            var splashScreen = new SplashScreen("img/icon.png");
            splashScreen.Show(false);
            System.Threading.Thread.Sleep(1000);
            splashScreen.Close(TimeSpan.FromSeconds(1));

            this.businessLayer = new BusinessLayer();
            this.businessLayerCodeGen = new BL.CodeGen.BusinessLayerCodeGen();

            Actions = new List<Models.Action>();
            
            InitializeComponent();

            RebindPropertyGrid();
            RebindListBox();

            // Initialize Properties Window.
            ComboBoxKeyPressKeyType.ItemsSource = IBusinessLayer.Keys;
            ComboBoxKeyPressKeyType.SelectedIndex = IBusinessLayer.KeysStartIndex;
            ComboBoxKeyPressPressType.ItemsSource = IBusinessLayer.KeyPressTypes;
            ComboBoxKeyPressPressType.SelectedIndex = IBusinessLayer.KeyPressTypesStartIndex;
            ComboBoxKeyListenerKeyType.ItemsSource = IBusinessLayer.Keys;
            ComboBoxKeyListenerKeyType.SelectedIndex = IBusinessLayer.KeysStartIndex;
            TextBoxMouseMoveXPosition.Text = IBusinessLayer.MouseMoveDefaultX.ToString();
            TextBoxMouseMoveYPosition.Text = IBusinessLayer.MouseMoveDefaultY.ToString();
            ComboBoxMouseClickButton.ItemsSource = IBusinessLayer.MouseButtonTypes;
            ComboBoxMouseClickButton.SelectedIndex = IBusinessLayer.MouseButtonTypesStartIndex;
            ComboBoxMouseClickClickType.ItemsSource = IBusinessLayer.KeyPressTypes;
            ComboBoxMouseClickClickType.SelectedIndex = IBusinessLayer.KeyPressTypesStartIndex;
            TextBoxPause.Text = IBusinessLayer.DefaultPauseDuration.ToString();
            TextBoxCharacterSequence.Text = IBusinessLayer.DefaultCharacterSequence.ToString();
            ComboBoxMediaKeyKeyType.ItemsSource = IBusinessLayer.MediaKeyTypes;
            ComboBoxMediaKeyKeyType.SelectedIndex = IBusinessLayer.MediaKeyTypesStartIndex;
            TextBoxHotStringInputString.Text = IBusinessLayer.DefaultHotStringInputString;
            TextBoxHotStringOutputString.Text = IBusinessLayer.DefaultHotStringOutputString;

            LabelProjectName.Content = IBusinessLayer.DefaultProjectName;
            LabelTimeOfLastSave.Content = IBusinessLayer.DefaultTimeSaved;

            /**
             * This will run on loop every second on another thread, occasionally
             * it will tell the UI thread to check if the hotkey processes were
             * killed externally, if that did happen, it will fix the UI's state.
             */
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    this.Dispatcher.Invoke(() =>
                    {
                        if(ButtonStopScript.IsEnabled && !businessLayerCodeGen.AnyRunningScripts())
                        {
                            ButtonPlayScript.IsEnabled = true;
                            ButtonStopScript.IsEnabled = false;
                            ButtonPlayScript.Visibility = Visibility.Visible;
                            ButtonStopScript.Visibility = Visibility.Hidden;
                        }
                    });
                }
            });
        }


        // Bindings
        /// <summary>
        /// Updates the visual list box to match the data in the stored list field.
        /// </summary>
        public void RebindListBox()
        {
            Actions = Actions.OrderBy(i => i.ActionPosition).ToList();

            ListBoxActions.ItemsSource = null;
            ListBoxActions.ItemsSource = Actions;
        }

        /// <summary>
        /// Essentially updates the Main Window to correctly display the current user.
        /// </summary>
        public void RebindUser()
        {
            if (LoggedInUser != null)
            {
                ButtonLoginLogout.Content = "LOGOUT";
                LabelNameOfUser.Content = LoggedInUser.ProfileName;
            }
            else
            {
                ButtonLoginLogout.Content = "SIGN IN";
                LabelNameOfUser.Content = "";
            }
        }

        /// <summary>
        /// Loads the corresponding properties window based on the selected action in the action sequence list box.
        /// </summary>
        /// <param name="action">The action to load to the properties window</param>
        public void RebindPropertyGrid(Models.Action action = null)
        {
            // Hide ALL property panes
            GridAllProperties.Children.OfType<Grid>().ToList().ForEach(i => i.Visibility = Visibility.Hidden);

            if (action != null)
            {
                // Turning this on prevents the event handler for the property dropdowns to trigger.
                ignorePropertyChanges = true;

                if (action.ActionTypeId == (int)ActionTypeEnum.KeyPress)
                {
                    // BindKeyPressProperty object to property window
                    ComboBoxKeyPressKeyType.SelectedValue = ((KeyPressProperty)action.Property).KeyPressed;
                    ComboBoxKeyPressPressType.SelectedValue = ((KeyPressProperty)action.Property).PressType;

                    GridKeyPressProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.KeyListener)
                {
                    // BindKeyListenerProperty object to property window
                    ComboBoxKeyListenerKeyType.SelectedValue = ((KeyListenerProperty)action.Property).Key;

                    GridKeyListenerProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.MouseMove)
                {
                    // BindMouseMoveProperty object to property window
                    TextBoxMouseMoveXPosition.Text = ((MouseMoveProperty)action.Property).Xposition.ToString();
                    TextBoxMouseMoveYPosition.Text = ((MouseMoveProperty)action.Property).Yposition.ToString();

                    GridMouseMoveProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.MouseClick)
                {
                    // BindMouseClickProperty object to property window
                    ComboBoxMouseClickButton.SelectedValue = ((MouseClickProperty)action.Property).Button;
                    ComboBoxMouseClickClickType.SelectedValue = ((MouseClickProperty)action.Property).ClickType;

                    GridMouseClickProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.Pause)
                {
                    // BindPauseProperty object to property window
                    TextBoxPause.Text = ((PauseProperty)action.Property).PauseDuration.ToString();

                    GridPauseProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.MediaKey)
                {
                    // Bind MediaKeyProperty object to property window
                    ComboBoxMediaKeyKeyType.SelectedItem = ((MediaKeyProperty)action.Property).MediaKey.ToString();

                    GridMediaKeyProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.CharacterSequence)
                {
                    // Bind CharacterSequenceProperty object to property window
                    TextBoxCharacterSequence.Text = ((CharacterSequenceProperty)action.Property).CharacterSequence.ToString();

                    GridCharacterSequenceProperties.Visibility = Visibility.Visible;
                }
                else if (action.ActionTypeId == (int)ActionTypeEnum.HotString)
                {
                    // Bind HotStringProperty object to property window
                    TextBoxHotStringInputString.Text
                        = ((HotStringProperty)action.Property).InputString.ToString();
                    TextBoxHotStringOutputString.Text
                        = ((HotStringProperty)action.Property).OutputString.ToString();

                    GridHotStringProperties.Visibility = Visibility.Visible;
                }

                // Turn this back off so the event handler for properties is allowed to trigger again.
                ignorePropertyChanges = false;
            }
        }


        // Buttons
        /// <summary>
        /// Plays the current sequence of actions in the script sequence.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlayScript_Click(object sender, RoutedEventArgs e)
        {
            if(Actions != null || Actions.Count != 0)
            {
                businessLayerCodeGen.Run(Actions);
                if(businessLayerCodeGen.AnyRunningScripts())
                {
                    ButtonPlayScript.IsEnabled = false;
                    ButtonStopScript.IsEnabled = true;
                    ButtonPlayScript.Visibility = Visibility.Hidden;
                    ButtonStopScript.Visibility = Visibility.Visible;
                }
            }
        }
        
        /// <summary>
        /// Stops the current running script. This is disabled when no script is running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStopScript_Click(object sender, RoutedEventArgs e)
        {
            if(businessLayerCodeGen.StopAllScripts())
            {
                ButtonPlayScript.IsEnabled = true; 
                ButtonStopScript.IsEnabled = false;
                ButtonPlayScript.Visibility = Visibility.Visible;
                ButtonStopScript.Visibility = Visibility.Hidden;
            }
        }
        
        /// <summary>
        /// Moves a selected action up in the action sequence.
        /// Note, if there is nothing selected, nothing happens.
        /// Note, if the action cannot be moved upward any further, nothing happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMoveActionUp_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxActions.SelectedItem == null)
            {
                MessageBox.Show("No action is selected.");
            }
            else if (ListBoxActions.Items.Count == 1)
            {
                return;
            }
            else
            {
                if (Actions.OrderBy(i => i.ActionPosition).First().ActionPosition
                    == ((Models.Action)ListBoxActions.SelectedItem).ActionPosition)
                {
                    MessageBox.Show("This action is already at the top.");
                    return; // If we hit this, that means the action we are trying to move UP is already the topmost action.
                }
                else
                {
                    int currentSelectedIndex = ListBoxActions.SelectedIndex;

                    Models.Action moveDown = Actions.Where(i => i.ActionPosition == 
                        (int)((Models.Action)ListBoxActions.SelectedItem).ActionPosition - 1).FirstOrDefault();
                    Models.Action moveUp = (Models.Action)ListBoxActions.SelectedItem;

                    SwapActions(moveUp, moveDown);
                    ListBoxActions.SelectedIndex = currentSelectedIndex - 1;

                    redoStack = new Stack<List<Models.Action>>();
                    undoStack.Push(mostRecentActions);
                    mostRecentActions = Actions;
                }
            }
        }
        
        /// <summary>
        /// Moves a selected action down in the action sequence.
        /// Note, if there is nothing selected, nothing happens.
        /// Note, if the action cannot be moved downward any further, nothing happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMoveActionDown_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxActions.SelectedItem == null)
            {
                MessageBox.Show("No action is selected.");
            }
            else if (ListBoxActions.Items.Count == 1)
            {
                return;
            }
            else
            {
                if (Actions.OrderBy(i => i.ActionPosition).Last().ActionPosition
                    == ((Models.Action)ListBoxActions.SelectedItem).ActionPosition)
                {
                    MessageBox.Show("This action is already at the bottom.");
                    return; // If we hit this, that means the action we are trying to move UP is already the topmost action.
                }
                else
                {
                    int currentSelectedIndex = ListBoxActions.SelectedIndex;

                    Models.Action moveUp = Actions.Where(i => i.ActionPosition ==
                        (int)((Models.Action)ListBoxActions.SelectedItem).ActionPosition + 1).FirstOrDefault();
                    Models.Action moveDown = (Models.Action)ListBoxActions.SelectedItem;

                    SwapActions(moveUp, moveDown);
                    ListBoxActions.SelectedIndex = currentSelectedIndex + 1;

                    redoStack = new Stack<List<Models.Action>>();
                    undoStack.Push(mostRecentActions);
                    mostRecentActions = Actions;
                }
            }
        }
        
        /// <summary>
        /// Swaps two of the actions in the action sequence list box (by sequence ordering).
        /// Also refresh the action sequence listbox after.
        /// </summary>
        /// <param name="moveUp"></param>
        /// <param name="moveDown"></param>
        private void SwapActions(Models.Action moveUp, Models.Action moveDown)
        {
            int tmpPosition = (int)moveUp.ActionPosition;
            moveUp.ActionPosition = moveDown.ActionPosition;
            moveDown.ActionPosition = tmpPosition;

            Actions = Actions.OrderBy(i => i.ActionPosition).ToList();
            RebindListBox();
        }
        
        /// <summary>
        /// Deletes the currently selected action from the list sequence.
        /// If one isn't selected, warn the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteAction_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxActions.SelectedItem == null)
            {
                MessageBox.Show("No action is selected.");
            }
            else
            {
                int deletedSequenceActionPosition = (int)((Models.Action)ListBoxActions.SelectedItem).ActionPosition;

                Actions.Remove(Actions.Where(i => i.ActionPosition == deletedSequenceActionPosition).FirstOrDefault());

                foreach (Models.Action action in Actions)
                {
                    if (action.ActionPosition > deletedSequenceActionPosition)
                    {
                        action.ActionPosition -= 1;
                    }
                }

                redoStack = new Stack<List<Models.Action>>();
                undoStack.Push(mostRecentActions);
                mostRecentActions = Actions;

                RebindListBox();
                RebindPropertyGrid();
            }
        } 

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRecordSequence_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Recording an action is not implemented yet.", "INFORMATIONAL");
        }
        
        /// <summary>
        /// Opens the add action window.
        /// Upon the window closing, if an actionType was selected, a new Action object is made and 
        /// placed in the action sequence.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddAction_Click(object sender, RoutedEventArgs e)
        {
            AddActionWindow addActionWindow = new AddActionWindow(businessLayer);
            addActionWindow.ShowDialog();

            // If the user were to exit the window an not select anything, this would hold a -1 value.
            // If it isn't -1, that means that they selected an ActionType and we want to create it.
            if (addActionWindow.ActionType != 0)
            {
                ActionTypeEnum selectedActionType = (ActionTypeEnum)addActionWindow.ActionType;

                int actionPosition;
                
                if (Actions != null && Actions.Count > 0)
                {
                    actionPosition = (int)Actions.OrderBy(i => i.ActionPosition).LastOrDefault().ActionPosition + 1;
                }
                else
                {
                    actionPosition = 1;
                }

                Property property = null;

                // Eventually migrate this to BL as a initialization method
                if (selectedActionType == ActionTypeEnum.KeyPress)
                {
                    property = new KeyPressProperty() { KeyPressed = IBusinessLayer.Keys[IBusinessLayer.KeysStartIndex], 
                        PressType = IBusinessLayer.KeyPressTypes[IBusinessLayer.KeyPressTypesStartIndex] };
                }
                else if (selectedActionType == ActionTypeEnum.KeyListener)
                {
                    property = new KeyListenerProperty() { Key = IBusinessLayer.Keys[IBusinessLayer.KeysStartIndex] };
                }
                else if (selectedActionType == ActionTypeEnum.MouseMove)
                {
                    property = new MouseMoveProperty() { Xposition = IBusinessLayer.MouseMoveDefaultX, 
                        Yposition = IBusinessLayer.MouseMoveDefaultY };
                }
                else if (selectedActionType == ActionTypeEnum.MouseClick)
                {
                    property = new MouseClickProperty(){ Button = IBusinessLayer.MouseButtonTypes[IBusinessLayer.MouseButtonTypesStartIndex], 
                        ClickType = IBusinessLayer.KeyPressTypes[IBusinessLayer.KeyPressTypesStartIndex] };
                }
                else if (selectedActionType == ActionTypeEnum.MediaKey)
                {
                    property = new MediaKeyProperty() { MediaKey = IBusinessLayer.MediaKeyTypes[IBusinessLayer.MediaKeyTypesStartIndex] }; // TODO
                }
                else if (selectedActionType == ActionTypeEnum.Pause)
                {
                    property = new PauseProperty() { PauseDuration = IBusinessLayer.DefaultPauseDuration }; // TODO
                }
                else if (selectedActionType == ActionTypeEnum.CharacterSequence)
                {
                    property = new CharacterSequenceProperty() { CharacterSequence = IBusinessLayer.DefaultCharacterSequence }; // TODO
                }
                else if (selectedActionType == ActionTypeEnum.HotString)
                {
                    property = new HotStringProperty() { InputString = IBusinessLayer.DefaultHotStringInputString, 
                        OutputString = IBusinessLayer.DefaultHotStringOutputString };
                }

                Actions.Add(new Models.Action() { Id = 0, ScriptId = 0, ActionTypeId = (int)selectedActionType, 
                    ActionPosition = actionPosition, Property = property});


                // Redo and Undo operations
                redoStack = new Stack<List<Models.Action>>();
                undoStack.Push(mostRecentActions);
                mostRecentActions = Actions;

                RebindListBox();
            }
        }
        
        /// <summary>
        /// If the user is not logged in, brings the user to the login screen.
        /// If the user is logged in, the user is logged out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLoginLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonLoginLogout.Content.Equals("Logout") || LoggedInUser != null)
                {
                    LoggedInUser = businessLayer.Logout();
                }
                else
                {
                    LoginWindow lw = new LoginWindow(businessLayer, LoggedInUser);
                    lw.ShowDialog();
                    this.LoggedInUser = lw.User;
                }

                RebindUser();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                throw;
            }
        }


        // Main List Box of Actions (aka "Action Sequence" for the current Script)
        /// <summary>
        /// When the action sequence list box has an index changed, this function tests if anything is selected
        /// and then 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxActions.SelectedItem != null)
            {
                RebindPropertyGrid((Models.Action)ListBoxActions.SelectedItem);
            }
            else
            {
                RebindPropertyGrid();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, e); 
        }
        
        /// <summary>
        /// Upon the window closing, this method is called.
        /// The method confirms if the user actually does want to close the window with scripts open or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            try
            {
                bool cancel = false;
                if (businessLayerCodeGen.AnyRunningScripts())
                {
                    ClosingConfirmationWindow cw = new ClosingConfirmationWindow();
                    cw.ShowDialog();
                    if (cw.cancel)
                    {
                        cancel = true;
                    }
                    else
                    {
                        this.businessLayerCodeGen.StopAllScripts();
                    }
                }

                if (cancel == false)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.ToString());
                MessageBox.Show("An error has occured while trying to exit the application: " + ex.ToString());
            }
        }
        
        /// <summary>
        /// Event handler for the minimize functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Event handler for allowing dragging of the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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