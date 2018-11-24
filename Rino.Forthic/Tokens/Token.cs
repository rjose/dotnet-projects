using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    public enum TokenType
    {
        COMMENT,
        STRING,
        WORD,
        START_ARRAY,
        END_ARRAY,
        START_MODULE,
        END_MODULE,
        DEFINITION_NAME
    };

    /// <summary>
    /// </summary>
    public class Token
    {
        public TokenType Type { get; }

        public Token(TokenType type)
        {
            Type = type;
        }
    }
}
