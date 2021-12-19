/* Author: Matthew Kotras
 * 
 * Description: This file contains all the logic pertaining to linking code generation to the rest of the program.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using ScriptBuddy.Models;
using ScriptBuddy.BL.CodeGen.Models;

namespace ScriptBuddy.BL.CodeGen
{
    public class BusinessLayerCodeGen
    {
        List<Process> _processList;

        public BusinessLayerCodeGen()
        {
            _processList = new List<Process>();
        }

        /// <summary>
        /// Converts a script into an executable .Ahk files.
        /// </summary>
        /// <param name="script">The script object to be exported.</param>
        /// <param name="filePath">The file path to the outgoing .ahk file (including the filename.ahk)</param>
        private void WriteScriptToAhk(string scriptText, string filePath)
        {
            //string ahkCode = "x:: Send, This script is working correctly!\n::btw::by the way";
            string[] output = scriptText.Split("\n");
            File.WriteAllLines($"{filePath}", output);
        }

        /// <summary>
        /// Converts an .ahk file script to an executable .exe.
        /// </summary>
        /// <param name="inFilePath">The path to the incoming ahk file (including the filename.ahk).</param>
        /// <param name="outFilePath">The path to the outgoing exe file (including the filename.exe).</param>
        /// <returns>True or False whether or not the compiling succeeded.</returns>
        private bool Ahk2Exe(string inFilePath, string outFilePath)
        {
            try
            {
                Process compiler = new Process();
                compiler.StartInfo.FileName = "./Compiler/Ahk2Exe.exe";
                compiler.StartInfo.Arguments = $"/in {inFilePath} /out {outFilePath} /silent";
                compiler.Start();
                compiler.WaitForExit();
                return compiler.ExitCode == 0;
            }
            catch (Exception ex)
            {
                //Should we display anything here if it fails?
            }
            return false;
        }

        /// <summary>
        /// Starts an executable file.
        /// </summary>
        /// <param name="filePath">TThe full file path to the executable (including the filename.exe)</param>
        /// <returns>True or False whether or not the executable could start.</returns>
        private Process StartExe(string filePath)
        {
            Process hotkey = new Process();
            hotkey.StartInfo.FileName = $"{filePath}";
            if (hotkey.Start())
            {
                return hotkey;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if there are any scripts (That the program knows about) running.
        /// </summary>
        /// <returns>Returns true if there are any registered scripts still running</returns>
        public bool AnyRunningScripts()
        {
            _processList.ForEach(p => p.Refresh());
            _processList.RemoveAll(p => p.HasExited);
            return _processList.Count != 0;
        }

        private void runScript(string scriptCode)
        {
            string ahkName = "./testScript.ahk";
            string exeName = "./testScript.exe";
            WriteScriptToAhk(scriptCode, ahkName);

            if (Ahk2Exe(ahkName, exeName))
            {
                Process p = StartExe(exeName);
                if (p != null)
                {
                    _processList.Add(p);
                }
            }
        }

        /// <summary>
        /// Attempts to kill and running script processes.
        /// </summary>
        /// <returns>Returns true only if all registered scripts are killed.</returns>
        public bool StopAllScripts()
        {
            foreach (Process p in _processList)
            {
                p.Kill();
                p.WaitForExit();
            }
            _processList.RemoveAll(p => p.HasExited);

            return _processList.Count == 0;
        }

        ///TODO: expand this to go beyond single key presses.
        private IAction ConvertToIAction(ScriptBuddy.Models.Action action)
        {
            Dictionary<Type, int> typeDict = new Dictionary<Type, int>()
            {
                { typeof(KeyPressProperty), 1 },
                { typeof(MouseMoveProperty), 3 },
                { typeof(MouseClickProperty), 4 },
                { typeof(MediaKeyProperty), 5 },
                { typeof(CharacterSequenceProperty), 6 },
                { typeof(PauseProperty), 7 }
            };
            Type t = action.Property.GetType();

            switch (typeDict.ContainsKey(t) ? typeDict[t] : -1)
            {
                case 1:
                    KeyPressProperty keyPressProperty = (KeyPressProperty) action.Property;
                    //Convert to a code gen presstype.
                    PressType keyPressType;
                    switch(keyPressProperty.PressType)
                    {
                        case "DOWN":
                            keyPressType = PressType.Down;
                            break;
                        case "UP":
                            keyPressType = PressType.Up;
                            break;
                        case "CLICK":
                            keyPressType = PressType.Tap;
                            break;
                        default:
                            keyPressType = PressType.Tap;
                            break;
                    }
                    return new SinglePress(keyPressProperty.KeyPressed[0], keyPressType);
                case 3:
                    MouseMoveProperty mouseMoveProperty = (MouseMoveProperty)action.Property;
                    return new MoveMouse(mouseMoveProperty.Xposition, mouseMoveProperty.Yposition);
                case 4:
                    MouseClickProperty mouseClickProperty = (MouseClickProperty)action.Property;
                    ClickType clickType;
                    switch(mouseClickProperty.Button)
                    {
                        case "Left Mouse Button":
                            clickType = ClickType.Left;
                            break;
                        case "Right Mouse Button":
                            clickType = ClickType.Right;
                            break;
                        case "Middle Mouse Button":
                            clickType = ClickType.Right;
                            break;
                        default:
                            clickType = ClickType.Left;
                            break;

                    }
                    PressType holdType;
                    switch (mouseClickProperty.ClickType)
                    {
                        case "DOWN":
                            holdType = PressType.Down;
                            break;
                        case "UP":
                            holdType = PressType.Up;
                            break;
                        case "CLICK":
                            holdType = PressType.Tap;
                            break;
                        default:
                            holdType = PressType.Tap;
                            break;
                    }
                    return new Click(clickType, holdType);
                //return new Click()
                case 5:
                    MediaKeyProperty mediaKeyProperty = (MediaKeyProperty)action.Property;
                    MediaKey key;
                    switch(mediaKeyProperty.MediaKey)
                    {
                        case "Play/Pause":
                            key = MediaKey.PlayPause;
                            break;
                        case "Next":
                            key = MediaKey.Next;
                            break;
                        case "Previous":
                            key = MediaKey.Previous;
                            break;
                        case "Volume Up":
                            key = MediaKey.VolumeUp;
                            break;
                        case "Volume Down":
                            key = MediaKey.VolumeDown;
                            break;
                        case "Mute":
                            key = MediaKey.Mute;
                            break;
                        default:
                            key = MediaKey.PlayPause;
                            break;
                    }
                    return new PressMediaKey( key );
                case 6:
                    CharacterSequenceProperty characterSequenceProperty = (CharacterSequenceProperty)action.Property;
                    return new TypeSequence(characterSequenceProperty.CharacterSequence);
                case 7:
                    PauseProperty pauseProperty = (PauseProperty)action.Property;
                    return new Sleep(pauseProperty.PauseDuration);
                default:
                        return null;
            }
        }

        /// <summary>
        /// Builds a list of IActions starting from the input and ending 
        /// with the iterator at the position of the next listener.
        /// </summary>
        /// <param name="actionSequence">The list of UI action objects</param>
        /// <param name="i">The starting position.</param>
        /// <returns></returns>
        public (int i, List<IAction> list) buildIActionsUntilListener(List<ScriptBuddy.Models.Action> actionSequence, int i)
        {
            Type[] listenerTypeProperties = new Type[] { typeof(KeyListenerProperty) };
            List<IAction> actionList = new List<IAction>();
            //Kinda weird code with the stop=false gobbledygook? -Matthew
            bool stop = false;
            while (!stop && i < actionSequence.Count)
            {
                Type curPropertyType = actionSequence[i].Property.GetType();
                foreach (Type listenerType in listenerTypeProperties)
                {
                    if (curPropertyType == listenerType)
                    {
                        stop = true;
                    }
                }
                if(!stop)
                {
                    actionList.Add(ConvertToIAction(actionSequence[i]));
                    i++;
                }
            }
            return (i, actionList);
        }

        /// <summary>
        /// Converts a series of business layer actions into ahk code.
        /// </summary>
        /// <param name="actionSequence"></param>
        /// <returns></returns>
        public string convertActionsToCode(List<ScriptBuddy.Models.Action> actionSequence)
        {
            List<ScriptBuddy.Models.Action> workingCopy = new List<ScriptBuddy.Models.Action>();
            List<ScriptBuddy.Models.Action> toRemove = new List<ScriptBuddy.Models.Action>();
            workingCopy.AddRange(actionSequence);

            //First, scan through the list and grab any hotstrings.
            List<HotString> hotStrings = new List<HotString>();
            workingCopy.ForEach(action =>
            {
                if(action.Property.GetType() == typeof(HotStringProperty))
                {
                    HotStringProperty hotStringProperty = (HotStringProperty) action.Property;
                    hotStrings.Add(new HotString(hotStringProperty.InputString, hotStringProperty.OutputString));
                    toRemove.Add(action);
                }
            });
            toRemove.ForEach(action => workingCopy.Remove(action));

            int i = 0;

            //Second, scan through the array and find the first listener,
            //everything before that must be on the initial execution step.

            var startingResult = buildIActionsUntilListener(workingCopy, 0);
            i = startingResult.i;
            List<IAction> startingActionList = startingResult.list;

            //At this point, i is sitting at either the first listener, or the end of the array.
            //And the startup code has been generated.

            List<ICodeExecutor> hotkeys = new List<ICodeExecutor>();

            while (i < workingCopy.Count)
            {
                ScriptBuddy.Models.Action curAction = workingCopy[i];
                //System.Windows.MessageBox.Show(i.ToString());
                //System.Windows.MessageBox.Show(curAction.Property.GetType().ToString());
                i++;
                var leadingActions = buildIActionsUntilListener(workingCopy, i);
                i = leadingActions.i;
                List<IAction> leadingActionsList = leadingActions.list;

                Dictionary<Type, int> listenerTypeDict = new Dictionary<Type, int>()
                {
                    { typeof(KeyListenerProperty), 0 },
                };
                


                Type listenerType = curAction.Property.GetType();

                switch (listenerTypeDict.ContainsKey(listenerType) ? listenerTypeDict[listenerType] : -1)
                {
                    case 0:
                        KeyListenerProperty keyListenerProperty = (KeyListenerProperty)curAction.Property;
                        List<HotkeyPrefix> prefixes = new List<HotkeyPrefix>();
                        string listenKey = keyListenerProperty.Key;
                        if (keyListenerProperty.Key.Length == 1)
                        {
                            char charKey = keyListenerProperty.Key[0];
                            if(char.IsUpper(charKey))
                            {
                                charKey = char.ToLower(charKey);
                                prefixes.Add(HotkeyPrefix.Shift);
                            }
                        }
                        else
                        {
                            switch(keyListenerProperty.Key)
                            {
                                case "SPACE":
                                    listenKey = "Space";
                                    break;
                                case "LSHIFT":
                                    listenKey = "LShift";
                                    break;
                                case "RSHIFT":
                                    listenKey = "RShift";
                                    break;
                                case "L CONTROL":
                                    listenKey = "LControl";
                                    break;
                                case "R CONTROL":
                                    listenKey = "RControl";
                                    break;
                                case "WINDOWS KEY":
                                    listenKey = "LWin";
                                    break;
                            }
                        }
                        SingleButtonHotkey sbhk = new SingleButtonHotkey(listenKey, prefixes, leadingActionsList);
                        hotkeys.Add(sbhk);
                        break;
                }
            }
            hotkeys.AddRange(hotStrings);
            ScriptBuddy.BL.CodeGen.Models.Script outScript = new ScriptBuddy.BL.CodeGen.Models.Script(startingActionList, hotkeys);
            //System.Windows.MessageBox.Show(outScript.GenerateCode(), "Generated Code");
            
            return outScript.GenerateCode();
        }

        public void Run(List<ScriptBuddy.Models.Action> actionSequence)
        {
            runScript(convertActionsToCode(actionSequence));
        }

        public void Stop()
        {

        }
    }
}
