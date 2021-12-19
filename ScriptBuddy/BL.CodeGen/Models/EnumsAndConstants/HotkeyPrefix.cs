/* Author: Matthew Kotras
 * 
 * Description: This file contains constants for hotkey prefixes.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Prefixes for single character hotkeys.
    /// </summary>
    public class HotkeyPrefix
    {
        public readonly string Prefix;
        public static HotkeyPrefix LWin = new("<#");
        public static HotkeyPrefix RWin = new(">#");
        public static HotkeyPrefix Ctrl = new("^");
        public static HotkeyPrefix Alt = new("!");
        public static HotkeyPrefix Shift = new("+");
        public static HotkeyPrefix LCtrl = new("<^");
        public static HotkeyPrefix RCtrl = new(">^");
        public static HotkeyPrefix LShift = new("<+");
        public static HotkeyPrefix RShift = new(">+");
        public static HotkeyPrefix LAlt = new("<!");
        public static HotkeyPrefix RAlt = new(">!");

        private HotkeyPrefix(string prefix)
        {
            this.Prefix = prefix;
        }

    }
}
