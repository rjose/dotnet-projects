using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class EndDefinitionToken : Token
    {
        public EndDefinitionToken() : base(TokenType.END_DEFINITION)
        {
        }
    }
}
