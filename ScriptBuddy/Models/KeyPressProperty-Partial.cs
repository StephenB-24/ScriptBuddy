using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class KeyPressProperty : Property
    {
        public override string ToString()
        {
            return "KeyPress -> " + this.PressType[0] + this.PressType.Substring(1, this.PressType.Length - 1).ToLower() + " the \'" + this.KeyPressed.ToLower() + "\' key";
        }
    }
}