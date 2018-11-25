using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    public delegate bool TryHandleLiteral(string text, out Word result);

    /// <summary>
    /// Modules store words and variables and handle literals. 
    /// </summary>
    public class Module : StackItem
    {
        protected List<Word> words;
        protected Dictionary<string, VariableItem> variables;
        protected List<TryHandleLiteral> literalHandlers;
        public string Name { get; }

        public Module(string name)
        {
            this.Name = name;
            words = new List<Word>();
            variables = new Dictionary<string, VariableItem>();
            literalHandlers = new List<TryHandleLiteral>();
        }

        public void AddWord(Word w)
        {
            words.Add(w);
        } 

        public void AddVariableIfMissing(string varname)
        {
            if (variables.ContainsKey(varname)) return;
            variables.Add(varname, new VariableItem());
        }

        public void AddLiteralHandler(TryHandleLiteral handler)
        {
            literalHandlers.Add(handler);
        }

        /// <summary>
        /// Searches for word in dictionary, then variables. If not found, tries
        /// treating as a literal.
        /// </summary>
        public bool TryFindWord(string text, out Word result)
        {
            bool found = false;
            result = null;

            found = TryFindDictionaryWord(text, out result);
            if (found) return found;

            found = TryFindVariable(text, out result);
            if (found) return found;

            found = TryLiteralHandlers(text, out result);
            if (found) return found;

            return false;
        }

        public bool TryFindDictionaryWord(string text, out Word result)
        {
            for (int i=words.Count-1; i >= 0; i--)
            {
                Word w = words[i];
                if (w.Text == text)
                {
                    result = w;
                    return true;
                }
            }
            result = null;
            return false;
        }

        public bool TryFindVariable(string text, out Word result)
        {
            bool found = false;

            if (variables.ContainsKey(text))
            {
                VariableItem variableItem = variables[text];
                result = new PushVariableItemWord("text", variableItem);
                found = true;
            }
            else
            {
                result = null;
                found = false;
            }
            return found;
        }

        public virtual bool TryLiteralHandlers(string text, out Word result)
        {
            bool found = false;

            for (int i=0; i < literalHandlers.Count; i++)
            {
                TryHandleLiteral handler = literalHandlers[i];
                found = handler(text, out result);
                if (found) return true;
            }

            result = null;
            return false;
        }
    }
}
