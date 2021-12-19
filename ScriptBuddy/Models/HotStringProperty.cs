using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class HotStringProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string InputString { get; set; }
        public string OutputString { get; set; }

        public virtual Action Action { get; set; }
    }
}
