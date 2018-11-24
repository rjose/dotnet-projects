using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class EndArrayToken : Token
    {
        public EndArrayToken() : base(TokenType.END_ARRAY)
        {
        }
    }
}
