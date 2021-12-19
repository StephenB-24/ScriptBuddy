/* Author: Matthew Kotras
 * 
 * Description: This file contains the Media key action.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// An action representing a press of a media key.
    /// </summary>
    public class PressMediaKey : IAction
    {
        private MediaKey _mediaKey;

        public PressMediaKey(MediaKey mediaKey)
        {
            _mediaKey = mediaKey;
        }

        public string GenerateCode()
        {
            return $"Send {{{_mediaKey.text}}}";
        }
    }
}
