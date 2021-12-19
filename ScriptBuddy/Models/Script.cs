using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class Script
    {
        public Script()
        {
            Actions = new HashSet<Action>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Accessibility { get; set; }
        public int CommunityTagId { get; set; }
        public DateTime TimeLastSaved { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
    }
}
