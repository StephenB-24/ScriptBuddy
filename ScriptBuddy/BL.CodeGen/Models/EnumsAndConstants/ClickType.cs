/* Author: Matthew Kotras
 * 
 * Description: This file contains constants for the different types of clicks.
 */

namespace ScriptBuddy.BL.CodeGen.Models
{
    /// <summary>
    /// Represents the different types of click.
    /// </summary>
    public class ClickType
    {
        public readonly string AsString;

        //M4 and M5 are generally the forward and backards keys respectively.
        public static ClickType Left = new("Left");
        public static ClickType Right = new("Right");
        public static ClickType Middle = new("Middle");
        public static ClickType M4 = new("X1");
        public static ClickType M5 = new("X2");

        private ClickType(string text)
        {
            this.AsString = text;
        }
    }
}
