using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class StartModuleToken : Token
    {
        public string Name { get ; }

        public StartModuleToken(string name) : base (TokenType.START_MODULE)
        {
            Name = name;
        }
    }
}
