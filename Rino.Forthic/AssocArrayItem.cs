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
        /// Enables MAP and FOREACH to be applied to AssocArrayItem
        /// </summary>
        public List<RecordItem> Items()
        {
            List<RecordItem> result = new List<RecordItem>();
            foreach(KeyValuePair<string, StackItem> entry in this.values)
            {
                RecordItem rec = new RecordItem();
                // rec.SetValue("key", entry.Key);  // TODO: Add a StringItem
                rec.SetValue("value", entry.Value);
                result.Add(rec);
            }
            return result;
        }
    }
}
