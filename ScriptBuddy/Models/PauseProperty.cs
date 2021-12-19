using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class PauseProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int PauseDuration { get; set; }

        public virtual Action Action { get; set; }
    }
}
