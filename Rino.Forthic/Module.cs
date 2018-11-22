using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Modules store words and variables and handle literals. 
    /// </summary>
    public class Module : StackItem
    {
        protected List<Word> words;
        protected Dictionary<string, VariableItem> variables;

        public Module()
        {
            this.words = new List<Word>();
            this.variables = new Dictionary<string, VariableItem>();
        }

       public void AddWord(Word w)
       {
           this.words.Add(w);
       } 

       public void AddVariableIfMissing(string varname)
       {
           if (this.variables.ContainsKey(varname))
           {
               return;
           }
           else
           {
               this.variables.Add(varname, new VariableItem());
           }
       }
    }
}
