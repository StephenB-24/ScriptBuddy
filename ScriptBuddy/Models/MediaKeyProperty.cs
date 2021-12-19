﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class MediaKeyProperty
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string MediaKey { get; set; }

        public virtual Action Action { get; set; }
    }
}
