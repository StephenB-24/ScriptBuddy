using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class HotStringProperty : Property
    {
        public override string ToString()
        {
            return "HotString -> When the user types \'" + this.InputString + "\' it will be replaced with \'" + this.OutputString + "\'";
        }
    }
}
