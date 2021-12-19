using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class MouseClickProperty : Property 
    {
        public override string ToString()
        {
            return "MouseClick -> " + this.ClickType[0] + this.ClickType.Substring(1, this.ClickType.Length - 1).ToLower() + " the " + this.Button.ToLower();
        }
    }
}