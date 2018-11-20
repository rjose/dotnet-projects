using System;
using System.Dynamic;

namespace Rino.Forthic
{
    /// <summary>
    /// A VariableItem holds a StackItem as a value.
    /// </summary>
    public class VariableItem : StackItem
    {
        public VariableItem()
        {
            this.VariableValue = null;
        }

        /// <summary>
        /// Gets and sets value
        /// </summary>
        public StackItem VariableValue { get; set; }
    }
}
