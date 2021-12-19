using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class Action
    {
        [NotMapped]
        public Models.Property Property { get; set; }

        public override string ToString()
        {
            Action a = this;

            ActionTypeEnum actionType = (ActionTypeEnum)ActionTypeId;
            string spacer = "";
            if (actionType != ActionTypeEnum.KeyListener && actionType != ActionTypeEnum.HotString)
            {
                spacer = "\t";
            }

            if (a.Property != null)
            {
                return  this.ActionPosition + spacer + "\t" + a.Property.ToString();
            }

            return "";
        }
    }
}