using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class CharacterSequenceProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string CharacterSequence { get; set; }

        public virtual Action Action { get; set; }
    }
}
