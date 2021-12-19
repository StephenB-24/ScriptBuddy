/* Author: Matthew Kotras
 * 
 * Description: This file contains the code generator interface.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents anything that can be converted into AHK code.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Returns the valid AHK code which this object represents.
        /// </summary>
        /// <returns>the AHK code in a string.</returns>
        public string GenerateCode();
    }
}
