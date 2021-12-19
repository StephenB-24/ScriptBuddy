/* Author: Matthew Kotras
 * 
 * Description: This file represents the ICodeExecutor.
 */

using System.Collections.Generic;
using System.Text;

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents a piece of code which causes a single button press to cause a sequence of events
    /// </summary>
    public class SingleButtonHotkey : ICodeExecutor
    {
        private string _activationText;
        private List<HotkeyPrefix> _hotkeyPrefixes;
        private List<IAction> _actions;

        public SingleButtonHotkey(string activationKey, List<HotkeyPrefix> prefixes, List<IAction> actions)
        {
            _activationText = activationKey;
            _hotkeyPrefixes = prefixes;
            _actions = actions;
        }
        public string GenerateCode()
        {
            StringBuilder codeStringBuilder = new StringBuilder();
            codeStringBuilder.Append("$");
            foreach(HotkeyPrefix hotkeyPrefix in _hotkeyPrefixes)
            {
                codeStringBuilder.Append(hotkeyPrefix.Prefix);
            }
            codeStringBuilder.Append(_activationText);
            codeStringBuilder.Append("::\n");
            foreach(IAction action in _actions)
            {
                codeStringBuilder.Append($"\t{action.GenerateCode()}\n");
            }
            codeStringBuilder.Append("\treturn");
            return codeStringBuilder.ToString();
        }
    }
}
