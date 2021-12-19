using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class MediaKeyProperty : Property
    {
        public override string ToString()
        {
            return "MediaKey -> Press " + this.MediaKey.ToLower();
        }
    }
}
