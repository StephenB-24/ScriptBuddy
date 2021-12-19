using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class MouseMoveProperty : Property 
    {
        public override string ToString()
        {
            return "MouseMove -> Move the mouse to x-position " + this.Xposition + " and y-position " + this.Yposition;
        }
    }
}
