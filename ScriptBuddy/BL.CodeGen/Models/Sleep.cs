/* Author: Matthew Kotras
 * 
 * Description: This file represents the sleep action.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// An action representing a sleep, where the code will stall for
    /// a set number of milliseconds.
    /// </summary>
    public class Sleep : IAction
    {
        private int _milliSeconds;

        public Sleep(int milliSeconds)
        {
            _milliSeconds = milliSeconds;
        }

        public string GenerateCode()
        {
            return $"Sleep, {_milliSeconds}";
        }
    }
}
