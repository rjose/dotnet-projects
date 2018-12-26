using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents a record object
    /// </summary>
    public class RecordItem : StackItem
    {
        protected Dictionary<string, StackItem> values;

        public RecordItem()
        {
            values = new Dictionary<string, StackItem>();
        }

        override public void SetValue(string key, StackItem value)
        {
            values[key] = value;
        }

        override public StackItem GetValue(string key)
        {
            return values[key];
        }
    }
}
