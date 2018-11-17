using System;
using System.Dynamic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents a string that's pushed onto the forthic stack.
    /// </summary>
    public class StringItem : StackItem
    {
        public StringItem(string value)
        {
            this.StringValue = value;
        }

        /// <summary>
        /// Gets value
        /// </summary>
        public string StringValue { get; }
    }
}
