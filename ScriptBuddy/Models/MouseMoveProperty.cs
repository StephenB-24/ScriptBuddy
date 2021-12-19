using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class MouseMoveProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int Xposition { get; set; }
        public int Yposition { get; set; }

        public virtual Action Action { get; set; }
    }
}
