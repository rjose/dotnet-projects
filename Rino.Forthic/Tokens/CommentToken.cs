using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class CommentToken : Token
    {
        public CommentToken() : base(TokenType.COMMENT)
        {
        }
    }
}
