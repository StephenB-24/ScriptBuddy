/* Author: Matthew Kotras
 * 
 * Description: This file contains constants for media keys.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents the code for each media key.
    /// </summary>
    public class MediaKey
    {
        public readonly string text;
        public static MediaKey PlayPause = new("Media_Play_Pause");
        public static MediaKey Previous = new("Media_Prev");
        public static MediaKey Next = new("Media_Next");
        public static MediaKey Mute = new("Volume_Mute");
        public static MediaKey VolumeUp = new("Volume_Up");
        public static MediaKey VolumeDown = new("Volume_Down");

        private MediaKey(string text)
        {
            this.text = text;
        }
    }
}
