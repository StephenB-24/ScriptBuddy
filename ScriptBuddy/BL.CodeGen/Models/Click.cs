/* Author: Matthew Kotras
 * 
 * Description: This class contains the Click action.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// An action representing a click of the mouse.
    /// </summary>
    public class Click : IAction
    {
        private ClickType _clickType;
        private PressType _keyPressType;

        public Click(ClickType clickType, PressType type)
        {
            _clickType = clickType;
            _keyPressType = type;
        }

        public string GenerateCode()
        {
            if (_keyPressType != PressType.Tap)
            {
                return $"Click {_clickType.AsString}, {_keyPressType}";
            }
            else
            {
                return $"Click {_clickType.AsString}";
            }
        }
    }
}
