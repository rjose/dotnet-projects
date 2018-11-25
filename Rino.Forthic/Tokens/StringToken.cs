using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class StringToken : Token
    {
        public string Text { get ; }

        public StringToken(string text) : base (TokenType.STRING)
        {
            Text = text;
        }
    }
}
