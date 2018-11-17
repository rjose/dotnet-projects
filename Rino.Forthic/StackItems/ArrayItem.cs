﻿using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents an associative array
    /// </summary>
    public class ArrayItem : StackItem
    {
        /// <summary>
        /// Gets value
        /// </summary>
        public List<StackItem> ArrayValue { get; }

        public ArrayItem() : base()
        {
            this.ArrayValue = new List<StackItem>();
        }

        public void AddItem(StackItem item)
        {
            this.ArrayValue.Add(item);
        }

        /// <summary>
        /// Enables MAP and FOREACH to be applied to ArrayItem.
        ///
        /// When mapping a word over an ArrayItem, the MAP word
        /// will call Items, push each item onto the stack, and then
        /// execute the mapping word.
        /// </summary>
        public List<StackItem> Items()
        {
            return this.ArrayValue;
        }
    }
}