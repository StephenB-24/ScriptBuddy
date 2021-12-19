/* Author: Matthew Kotras
 * 
 * Description: This file contains the Script class.
 */

using System.Collections.Generic;
using System.Text;

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents an entire autohotkey script which can encompass multiple different hotkeys
    /// which each may contain a multitude of actions associated.
    /// </summary>
    public class Script : ICodeGenerator
    {
        /// <summary>
        /// Code which runs as soon as the script is started.
        /// </summary>
        private List<IAction> _startupCode;

        /// <summary>
        /// All of the hotkeys which result from an input.
        /// </summary>
        private List<ICodeExecutor> _hotKeys;

        public Script(List<IAction> startupCode, List<ICodeExecutor> hotKeys)
        {
            _startupCode = startupCode;
            _hotKeys = hotKeys;
        }

        public string GenerateCode()
        {
            StringBuilder codeStringBuilder = new StringBuilder();
            foreach (IAction action in _startupCode)
            {
                codeStringBuilder.Append($"{action.GenerateCode()}\n");
            }

            foreach (ICodeGenerator hotkey in _hotKeys)
            {
                codeStringBuilder.Append($"{hotkey.GenerateCode()}\n");
            }
            return codeStringBuilder.ToString();
        }
    }
}
