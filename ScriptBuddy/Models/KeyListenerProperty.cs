using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class KeyListenerProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string Key { get; set; }
        public int ActionEndPosition { get; set; }

        public virtual Action Action { get; set; }
    }
}
