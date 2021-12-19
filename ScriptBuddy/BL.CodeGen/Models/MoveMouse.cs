/* Author: Matthew Kotras
 * 
 * Description: This file contains the Move Mouse action.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents an action of moving the mouse.
    /// </summary>
    public class MoveMouse : IAction
    {
        private int _x;
        private int _y;

        public MoveMouse(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public string GenerateCode()
        {
            return $"MouseMove, {_x}, {_y}";
        }
    }
}
