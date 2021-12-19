/* Author: Matthew Kotras
 * 
 * Description: This file represents a single press action.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents a single press of the keyboard.
    /// </summary>
    public class SinglePress : IAction
    {
        private PressType _keyPressType;
        private char _keyToPress;
        public SinglePress(char press, PressType type)
        {
            _keyToPress = press;
            _keyPressType = type;
        }

        public string GenerateCode()
        {
            if(_keyPressType != PressType.Tap)
            {
                return $"Send {{{_keyToPress} {_keyPressType}}}";
            }
            else
            {
                return $"Send {_keyToPress}";
            }
        }
    }
}
