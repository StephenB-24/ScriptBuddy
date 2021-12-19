using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class Action
    {
        public Action()
        {
            CharacterSequenceProperties = new HashSet<CharacterSequenceProperty>();
            HotStringProperties = new HashSet<HotStringProperty>();
            KeyListenerProperties = new HashSet<KeyListenerProperty>();
            KeyPressProperties = new HashSet<KeyPressProperty>();
            MediaKeyProperties = new HashSet<MediaKeyProperty>();
            MouseClickProperties = new HashSet<MouseClickProperty>();
            MouseMoveProperties = new HashSet<MouseMoveProperty>();
            PauseProperties = new HashSet<PauseProperty>();
        }

        public int Id { get; set; }
        public int ScriptId { get; set; }
        public int ActionTypeId { get; set; }
        public int ActionPosition { get; set; }

        public virtual Script Script { get; set; }
        public virtual ICollection<CharacterSequenceProperty> CharacterSequenceProperties { get; set; }
        public virtual ICollection<HotStringProperty> HotStringProperties { get; set; }
        public virtual ICollection<KeyListenerProperty> KeyListenerProperties { get; set; }
        public virtual ICollection<KeyPressProperty> KeyPressProperties { get; set; }
        public virtual ICollection<MediaKeyProperty> MediaKeyProperties { get; set; }
        public virtual ICollection<MouseClickProperty> MouseClickProperties { get; set; }
        public virtual ICollection<MouseMoveProperty> MouseMoveProperties { get; set; }
        public virtual ICollection<PauseProperty> PauseProperties { get; set; }
    }
}
