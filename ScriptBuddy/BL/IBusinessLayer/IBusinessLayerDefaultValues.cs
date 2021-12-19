/* Author:      Stephen Behnke
 * 
 * Description: This interface specifies all the start indexing/default values of the program.
 */

using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptBuddy.BL
{
    /// <summary>
    /// These are read only properties of the business layer interface (like keys, etc.).
    /// </summary>
    public partial interface IBusinessLayer
    {
        // Key Types and Starting Values Fields
        /// <summary>
        /// These are the keys of the keyboard that can be represented in Script Buddy.
        /// </summary>
        private static List<string> keys = new List<string>() { "a", "A", "b", "B", "c", "C", "d", "D", "e", "E", "f", "F", "g",
            "G", "h", "H", "i", "I", "j", "J", "k", "K", "l", "L", "m", "M", "n", "N", "o", "O", "p", "P", "q", "Q", "r", "R",
            "s", "S", "t", "T", "u", "U", "v", "V", "w", "W", "x", "X", "y", "Y", "z", "Z", "SPACE", "LSHIFT", "RSHIFT",
            "L CONTROL", "R CONTROL", "WINDOWS KEY" };
       
        /// <summary>
        /// The default selected index of the key types.
        /// </summary>
        private static int keysStartIndex = 0;
       
        /// <summary>
        /// The key press types for the keyboard and the mouse.
        /// </summary>
        private static List<string> keyPressTypes = new List<string>() { "Press Down", "Release", "Click" };
       
        /// <summary>
        /// The default media keys available.
        /// </summary>
        private static List<string> mediaKeyTypes = new List<string>() { "Play/Pause", "Next", "Previous", "Volume Up", "Volume Down", "Mute" };
       
        /// <summary>
        /// The default index within the media keys list to preselect.
        /// </summary>
        private static int mediaKeyTypesStartIndex = 0;
       
        /// <summary>
        /// The default selected index of the key press types.
        /// </summary>
        private static int keyPressTypesStartIndex = 2;
       
        /// <summary>
        /// The types of mouse buttons.
        /// </summary>
        private static List<string> mouseButtonTypes = new List<string>() { "Left Mouse Button", "Right Mouse Button", "Middle Mouse Button" };
        
        /// <summary>
        /// The default selected index of the mouse button types.
        /// </summary>
        private static int mouseButtonTypesStartIndex = 0;
        
        /// <summary>
        /// Default x position for a mouse move.
        /// </summary>
        private static int mouseMoveDefaultX = 0;
       
        /// <summary>
        /// Default y position for a mouse move.
        /// </summary>
        private static int mouseMoveDefaultY = 0;
        
        /// <summary>
        /// Default duration in milliseconds for a pause action.
        /// </summary>
        private static int defaultPauseDuration = 1000;
       
        /// <summary>
        /// Default character sequence for the character sequence object.
        /// </summary>
        private static string defaultCharacterSequence = "";
        
        /// <summary>
        /// Default input string hotstring for the hotstring converter.
        /// </summary>
        private static string defaultHotStringInputString = "btw";
       
        /// <summary>
        /// Default output string hotstring for the hotstring converter.
        /// </summary>
        private static string defaultHotStringOutputString = "by the way";
        
        /// <summary>
        /// The default new project name.
        /// </summary>
        private static string defaultProjectName = "Unnamed Project";
       
        /// <summary>
        /// The default time saved for a new project.
        /// </summary>
        private static string defaultTimeSaved = "Not Saved";


        // Key Types and Starting Values Properties
        /// <summary>
        /// These are the keys of the keyboard that can be represented in Script Buddy.
        /// </summary>
        public static List<string> Keys { get { return keys; } }
        
        /// <summary>
        /// The default selected index of the key types.
        /// </summary>
        public static int KeysStartIndex { get { return keysStartIndex; } }
        
        /// <summary>
        /// The key press types for the keyboard and the mouse.
        /// </summary>
        public static List<string> KeyPressTypes { get { return keyPressTypes; } }
        
        /// <summary>
        /// The default index within the media keys list to preselect.
        /// </summary>
        public static int MediaKeyTypesStartIndex { get { return mediaKeyTypesStartIndex; } }
        
        /// <summary>
        /// The default selected index of the key press types.
        /// </summary>
        public static int KeyPressTypesStartIndex { get { return keyPressTypesStartIndex; } }
        
        /// <summary>
        /// The default media keys available.
        /// </summary>
        public static List<string> MediaKeyTypes { get { return mediaKeyTypes; } }
        
        /// <summary>
        /// The types of mouse buttons.
        /// </summary>
        public static List<string> MouseButtonTypes { get { return mouseButtonTypes; } }
        
        /// <summary>
        /// The default selected index of the mouse button types.
        /// </summary>
        public static int MouseButtonTypesStartIndex { get { return mouseButtonTypesStartIndex; } }

        /// <summary>
        /// Default x position for a mouse move.
        /// </summary>
        public static int MouseMoveDefaultX { get { return mouseMoveDefaultX; } }

        /// <summary>
        /// Default y position for a mouse move.
        /// </summary>
        public static int MouseMoveDefaultY { get { return mouseMoveDefaultY; } }

        /// <summary>
        /// Default duration in milliseconds for a pause action.
        /// </summary>
        public static int DefaultPauseDuration { get { return defaultPauseDuration; } }

        /// <summary>
        /// Default character sequence for the character sequence object.
        /// </summary>
        public static string DefaultCharacterSequence { get { return defaultCharacterSequence; } }

        /// <summary>
        /// Default input string hotstring for the hotstring converter.
        /// </summary>
        public static string DefaultHotStringInputString { get { return defaultHotStringInputString; } }

        /// <summary>
        /// Default output string hotstring for the hotstring converter.
        /// </summary>
        public static string DefaultHotStringOutputString { get { return defaultHotStringOutputString; } }

        /// <summary>
        /// The default new project name.
        /// </summary>
        public static string DefaultProjectName { get { return defaultProjectName; } }

        /// <summary>
        /// The default time saved for a new project.
        /// </summary>
        public static string DefaultTimeSaved { get { return defaultTimeSaved; } }
    }
}
