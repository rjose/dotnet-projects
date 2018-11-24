using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class WordToken : Token
    {
        public string Text { get ; }

        public WordToken(string text) : base (TokenType.WORD)
        {
            Text = text;
        }
    }
}
