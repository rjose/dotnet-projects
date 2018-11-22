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
        public string Text { get; }

        public Word(string text)
        {
            this.Text = text;
        }

        public virtual void Execute(Interpreter interp)
        {
        }
    }
}
