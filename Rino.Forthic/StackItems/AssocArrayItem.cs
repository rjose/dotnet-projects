using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents an associative array
    /// </summary>
    public class AssocArrayItem : RecordItem
    {
        public AssocArrayItem() : base()
        {
        }

        /// <summary>
        /// Enables MAP and FOREACH to be applied to AssocArrayItem.
        ///
        /// When mapping a word over an AssocArrayItem, the MAP word
        /// will call Items, push each item onto the stack, and then
        /// execute the mapping word. The items for an AssocArrayItem
        /// will be RecordItems with fields "key" and "value" corresponding
        /// to each record.
        /// </summary>
        public List<RecordItem> Items()
        {
            List<RecordItem> result = new List<RecordItem>();
            foreach(KeyValuePair<string, StackItem> entry in this.values)
            {
                RecordItem rec = new RecordItem();
                rec.SetValue("key", new StringItem(entry.Key));
                rec.SetValue("value", entry.Value);
                result.Add(rec);
            }
            return result;
        }
    }
}
