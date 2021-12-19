using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class KeyListenerProperty : Property
    {
        public override string ToString()
        {
            return "KeyListener -> Listening for the \'" + this.Key + "\' key, then executing the following:";
        }
    }
}