using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class MouseClickProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string Button { get; set; }
        public string ClickType { get; set; }

        public virtual Action Action { get; set; }
    }
}
