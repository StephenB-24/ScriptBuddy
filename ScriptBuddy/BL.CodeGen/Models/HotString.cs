/* Author: Matthew Kotras
 * 
 * Description: This file contains the hotstring class.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// A hotstring is a specific type of macro which is commonly used to 
    /// expand acronyms. For instance, a hotstring of ("btw" -> "by the way")
    /// will automate typing so typing "btw " will replace that text with "by the way".
    /// </summary>
    public class HotString : ICodeExecutor
    {
        private string _activationString;
        private string _outputString;

        public HotString(string activationString, string outputString)
        {
            _activationString = activationString;
            _outputString = outputString;
        }

        public string GenerateCode()
        {
            return $"::{_activationString}::{_outputString}";
        }
    }
}
