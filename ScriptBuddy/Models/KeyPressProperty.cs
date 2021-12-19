using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class KeyPressProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string KeyPressed { get; set; }
        public string PressType { get; set; }

        public virtual Action Action { get; set; }
    }
}
