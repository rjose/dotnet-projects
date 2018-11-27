using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class StartDefinitionToken : Token
    {
        public string Name { get ; }

        public StartDefinitionToken(string text) : base (TokenType.START_DEFINITION)
        {
            Name = text;
        }
    }
}
