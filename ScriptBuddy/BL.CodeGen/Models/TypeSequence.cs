/* Author: Matthew Kotras
 * 
 * Description: This file represents an IAction.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents a type of action which simply types out a string.
    /// </summary>
    public class TypeSequence : IAction
    {
        private string _whatToType;

        public TypeSequence(string whatToType)
        {
            _whatToType = whatToType;
        }

        public string GenerateCode()
        {
            if(_whatToType.Length == 0)
            {
                return "";
            }
            else
            {
                return $"Send, {_whatToType}";
            }
        }
    }
}
