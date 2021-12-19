using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class PauseProperty : Property
    {
        public override string ToString()
        {
            return "Pause -> Pause execution for " + this.PauseDuration + " milliseconds";
        }
    }
}
