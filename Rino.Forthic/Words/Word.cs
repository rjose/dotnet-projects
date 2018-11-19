using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Base class for Forthic words 
    /// </summary>
    public class Word
    {
        public Word()
        {
        }

        public virtual void Execute(Interpreter interp)
        {
        }
    }
}
