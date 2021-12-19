using System;
using System.Collections.Generic;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class CharacterSequenceProperty : Property
    {
        public override string ToString()
        {
            string baseString = "CharacterSequence -> ";
            if (this.CharacterSequence == null || this.CharacterSequence.Equals(""))
            {
                return baseString + "Careful, this character sequence is blank and will not do anything!";
            }

            return baseString + "The computer will type: \'" + this.CharacterSequence + "\'";
        }
    }
}
