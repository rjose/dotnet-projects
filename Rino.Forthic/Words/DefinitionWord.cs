using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class DefinitionWord : Word
    {
        protected List<Word> words;

        public DefinitionWord(string text) : base(text)
        {
            words = new List<Word>();
        }

        public void CompileWord(Word word)
        {
            if (word is null) return;
            words.Add(word);
        }

        public override void Execute(Interpreter interp)
        {
            foreach(Word w in words)
            {
                w.Execute(interp);
            }
        }
    }
}
