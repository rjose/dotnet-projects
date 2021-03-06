﻿using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Modules store words and variables and handle literals. 
    /// </summary>
    public class GlobalModule : Module
    {
        public GlobalModule() : base("Forthic.GLOBAL")
        {
            AddLiteralHandler(TryHandleIntLiteral);
            AddLiteralHandler(TryHandleFloatLiteral);
            AddLiteralHandler(TryHandleBoolLiteral);

            AddWord(new UseModulesWord("USE-MODULES"));
            AddWord(new ImportWordsWord("IMPORT-WORDS"));
            AddWord(new AsWord("AS"));
            AddWord(new RecWord("REC"));
            AddWord(new RecAtWord("REC@"));
            AddWord(new RecBangWord("REC!"));
            AddWord(new RecBangKeepWord("<REC!"));
            AddWord(new ForeachWord("FOREACH"));
            AddWord(new MapWord("MAP"));
            AddWord(new PopWord("POP"));
            AddWord(new DupWord("DUP"));
            AddWord(new VariableWord("VARIABLE"));
            AddWord(new VariablesWord("VARIABLES"));
            AddWord(new BangWord("!"));
            AddWord(new AtWord("@"));
            AddWord(new SwapWord("SWAP"));
            AddWord(new NotWord("NOT"));
            AddWord(new LengthWord("LENGTH"));
            AddWord(new NthWord("NTH"));
            AddWord(new NullWord("NULL"));
            AddWord(new PowWord("POW"));
            AddWord(new FlattenWord("FLATTEN"));
            AddWord(new UnpackWord("UNPACK"));
            AddWord(new SortByFieldWord("SORT-BY-FIELD"));
            AddWord(new SignWord("SIGN"));
            AddWord(new LessThanWord("<"));
            AddWord(new GreaterThanWord(">"));
            AddWord(new PublishWord("PUBLISH"));
            AddWord(new ForWord("FOR"));
        }

        protected bool TryHandleIntLiteral(string text, out Word result)
        {
            bool found = false;
            try {
                int val = Int32.Parse(text);
                result = new PushStackItemWord(text, new IntItem(val));
                found = true;
            }
            catch {
                result = null;
                found = false;
            }

            return found;
        }

        protected bool TryHandleFloatLiteral(string text, out Word result)
        {
            bool found = false;
            try {
                double val = Double.Parse(text);
                result = new PushStackItemWord(text, new DoubleItem(val));
                found = true;
            }
            catch {
                result = null;
                found = false;
            }

            return found;
        }

        protected bool TryHandleBoolLiteral(string text, out Word result)
        {
            if (text == "true")
            {
                result = new PushStackItemWord(text, new BoolItem(true));
                return true;
            }
            else if (text == "false")
            {
                result = new PushStackItemWord(text, new BoolItem(false));
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }

    // -------------------------------------------------------------------------
    // Words

    class UseModulesWord : Word
    {
        public UseModulesWord(string name) : base(name)
        {
        }

        // ( modules -- )
        public override void Execute(Interpreter interp)
        {
            ArrayItem modules = (ArrayItem)interp.StackPop();
            foreach(Module m in modules.ArrayValue)
            {
                interp.UseModule(m);
            }
        }
    }

    class ImportWordsWord : Word
    {
        public ImportWordsWord(string name) : base(name) { }

        // ( module names -- )
        public override void Execute(Interpreter interp)
        {
            ArrayItem names = (ArrayItem)interp.StackPop();
            Module module = (Module)interp.StackPop();

            foreach(StackItem item in names.ArrayValue)
            {
                string name, newName;
                if (item is ArrayItem)
                {
                    ArrayItem arrayItem = (ArrayItem)item;
                    name = ((StringItem)arrayItem.ArrayValue[0]).StringValue;
                    newName = ((StringItem)arrayItem.ArrayValue[1]).StringValue;
                }
                else
                {
                    name = ((StringItem)item).StringValue;
                    newName = name;
                }

                Word word;
                if(!module.TryFindWord(name, out word))
                {
                    string msg = String.Format("Can't find word {0} in module '{1}'", name, module.Name);
                    throw new InvalidOperationException(msg);
                }

                word.Text = newName;
                interp.CurModule().AddWord(word);
            }
        }
    }

    class AsWord : Word
    {
        public AsWord(string name) : base(name) { }

        // ( a b -- [ a b ] )
        public override void Execute(Interpreter interp)
        {
            StackItem b = interp.StackPop();
            StackItem a = interp.StackPop();
            ArrayItem result = new ArrayItem();
            result.Add(a);
            result.Add(b);
            interp.StackPush(result);
        }
    }

    class RecWord : Word
    {
        public RecWord(string name) : base(name) { }

        // ( values fields -- record )
        public override void Execute(Interpreter interp)
        {
            ArrayItem fields = (ArrayItem)interp.StackPop();
            ArrayItem values = (ArrayItem)interp.StackPop();

            RecordItem result = new RecordItem();
            for(int i=0; i < fields.ArrayValue.Count; i++)
            {
                StringItem keyItem = (StringItem)fields.ArrayValue[i];
                result.SetValue(keyItem.StringValue, values.ArrayValue[i]);
            }
            interp.StackPush(result);
        }
    }

    class RecAtWord : Word
    {
        public RecAtWord(string name) : base(name) { }

        // ( record fieldname -- value )
        public override void Execute(Interpreter interp)
        {
            StringItem fieldname = (StringItem)interp.StackPop();
            StackItem record = interp.StackPop();
            interp.StackPush(record.GetValue(fieldname.StringValue));
        }
    }

    class RecBangKeepWord : Word
    {
        public RecBangKeepWord(string name) : base(name) { }

        // ( record value fieldname -- record )
        public override void Execute(Interpreter interp)
        {
            StringItem fieldName = (StringItem)interp.StackPop();
            StackItem valueItem = interp.StackPop();
            StackItem record = interp.StackPop();
            record.SetValue(fieldName.StringValue, valueItem);
            interp.StackPush(record);
        }
    }

    class RecBangWord : RecBangKeepWord
    {
        public RecBangWord(string name) : base(name) { }

        // ( record value fieldname -- )
        public override void Execute(Interpreter interp)
        {
            base.Execute(interp);
            interp.StackPop();
        }
    }

    class ForeachWord : Word
    {
        public ForeachWord(string name) : base(name) { }

        // ( items forthic -- )
        public override void Execute(Interpreter interp)
        {
            StringItem forthic = (StringItem)interp.StackPop();
            ArrayItem items = (ArrayItem)interp.StackPop();
            foreach(var item in items.ArrayValue)
            {
                interp.StackPush(item);
                interp.Run(forthic.StringValue);
            }
        }
    }

    class MapWord : Word
    {
        public MapWord(string name) : base(name) { }

        // ( items forthic -- new_items )
        public override void Execute(Interpreter interp)
        {
            StringItem forthic = (StringItem)interp.StackPop();
            ArrayItem items = (ArrayItem)interp.StackPop();

            ArrayItem result = new ArrayItem();
            foreach(var item in items.ArrayValue)
            {
                interp.StackPush(item);
                interp.Run(forthic.StringValue);
                result.Add(interp.StackPop());
            }
            interp.StackPush(result);
        }
    }

    class PopWord : Word
    {
        public PopWord(string name) : base(name) { }

        // ( a -- )
        public override void Execute(Interpreter interp)
        {
            interp.StackPop();
        }
    }

    class DupWord : Word
    {
        public DupWord(string name) : base(name) { }

        // ( a -- a a )
        public override void Execute(Interpreter interp)
        {
            StackItem a = interp.StackPop();
            interp.StackPush(a);
            interp.StackPush(a);
        }
    }

    class VariableWord : Word
    {
        public VariableWord(string name) : base(name) { }

        // ( name -- )
        public override void Execute(Interpreter interp)
        {
            StringItem name = (StringItem)interp.StackPop();
            interp.CurModule().AddVariableIfMissing(name.StringValue);
        }
    }

    class VariablesWord : Word
    {
        public VariablesWord(string name) : base(name) { }

        // ( names -- )
        public override void Execute(Interpreter interp)
        {
            ArrayItem names = (ArrayItem)interp.StackPop();
            foreach (StringItem item in names.ArrayValue)
            {
                interp.CurModule().AddVariableIfMissing(item.StringValue);
            }
        }
    }

    class BangWord : Word
    {
        public BangWord(string name) : base(name) { }

        // ( value variable -- )
        public override void Execute(Interpreter interp)
        {
            VariableItem variable = (VariableItem)interp.StackPop();
            StackItem value = interp.StackPop();
            variable.VariableValue = value;
        }
    }

    class AtWord : Word
    {
        public AtWord(string name) : base(name) { }

        // ( variable -- )
        public override void Execute(Interpreter interp)
        {
            VariableItem variable = (VariableItem)interp.StackPop();
            interp.StackPush(variable.VariableValue);
        }
    }

    class SwapWord : Word
    {
        public SwapWord(string name) : base(name) { }

        // ( a b -- b a )
        public override void Execute(Interpreter interp)
        {
            StackItem b = interp.StackPop();
            StackItem a = interp.StackPop();
            interp.StackPush(b);
            interp.StackPush(a);
        }
    }

    class NotWord : Word
    {
        public NotWord(string name) : base(name) { }

        // ( bool -- !bool )
        public override void Execute(Interpreter interp)
        {
            BoolItem value = (BoolItem)interp.StackPop();
            interp.StackPush(new BoolItem(!value.BoolValue));
        }
    }

    class LengthWord : Word
    {
        public LengthWord(string name) : base(name) { }

        // ( items -- length )
        public override void Execute(Interpreter interp)
        {
            ArrayItem items = (ArrayItem)interp.StackPop();
            interp.StackPush(length(items));
        }

        IntItem length(ArrayItem items)
        {
            return new IntItem(items.ArrayValue.Count);
        }
    }

    class NthWord : Word
    {
        public NthWord(string name) : base(name) { }

        // ( items n -- items[n] )
        public override void Execute(Interpreter interp)
        {
            IntItem n = (IntItem)interp.StackPop();
            ArrayItem items = (ArrayItem)interp.StackPop();
            interp.StackPush(items.ArrayValue[n.IntValue]);
        }
    }

    class NullWord : Word
    {
        public NullWord(string name) : base(name) { }

        // ( -- NullItem )
        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new NullItem());
        }
    }

    class PowWord : Word
    {
        public PowWord(string name) : base(name) { }

        // ( a b -- a^b )
        public override void Execute(Interpreter interp)
        {
            ScalarItem b = (ScalarItem)interp.StackPop();
            ScalarItem a = (ScalarItem)interp.StackPop();
            interp.StackPush(new DoubleItem(Math.Pow(a.DoubleValue, b.DoubleValue)));
        }
    }

    class UnpackWord : Word
    {
        public UnpackWord(string name) : base(name) { }

        // ( [a b ...] -- a b ... )
        public override void Execute(Interpreter interp)
        {
            ArrayItem items = (ArrayItem)interp.StackPop();
            foreach (StackItem item in items.ArrayValue)
            {
                interp.StackPush(item);
            }
        }
    }

    class FlattenWord : Word
    {
        public FlattenWord(string name) : base(name) { }

        // ( list-of-lists -- list )
        public override void Execute(Interpreter interp)
        {
            ArrayItem list_of_lists = (ArrayItem)interp.StackPop();
            List<StackItem> result = new List<StackItem>();
            flatten(list_of_lists, result);
            interp.StackPush(new ArrayItem(result));
        }

        void flatten(ArrayItem items, List<StackItem> res)
        {
            foreach (StackItem item in items.ArrayValue)
            {
                if (item.GetType() == typeof(ArrayItem))
                {
                    flatten((ArrayItem)item, res);
                }
                else
                {
                    res.Add(item);
                }
            }
        }
    }

    class SortByFieldWord : Word
    {
        public SortByFieldWord(string name) : base(name) { }

        // ( items field -- sorted-items )
        public override void Execute(Interpreter interp)
        {
            StringItem field = (StringItem)interp.StackPop();
            ArrayItem items = (ArrayItem)interp.StackPop();
            List<StackItem> item_list = (List<StackItem>)items.ArrayValue;
            sort(item_list, field.StringValue);
            interp.StackPush(new ArrayItem(item_list));
        }

        void sort(List<StackItem> items, string field)
        {
            int sortByField(StackItem l, StackItem r)
            {
                StackItem l_val = l.GetValue(field);
                StackItem r_val = r.GetValue(field);

                return l_val.CompareTo(r_val);
            }
            items.Sort(sortByField);
        }
    }

    class SignWord : Word
    {
        public SignWord(string name) : base(name) { }

        // ( a -- sign )
        public override void Execute(Interpreter interp)
        {
            ScalarItem a = (ScalarItem)interp.StackPop();
            interp.StackPush(new IntItem(Math.Sign(a.IntValue)));
        }
    }

    class LessThanWord : Word
    {
        public LessThanWord(string name) : base(name) { }

        // ( a b -- bool )
        public override void Execute(Interpreter interp)
        {
            ScalarItem b = (ScalarItem)interp.StackPop();
            ScalarItem a = (ScalarItem)interp.StackPop();
            interp.StackPush(new BoolItem(a.DoubleValue < b.DoubleValue));
        }
    }

    class GreaterThanWord : Word
    {
        public GreaterThanWord(string name) : base(name) { }

        // ( a b -- bool )
        public override void Execute(Interpreter interp)
        {
            ScalarItem b = (ScalarItem)interp.StackPop();
            ScalarItem a = (ScalarItem)interp.StackPop();
            interp.StackPush(new BoolItem(a.DoubleValue > b.DoubleValue));
        }
    }

    class PublishWord : Word
    {
        public PublishWord(string name) : base(name) { }

        // ( words -- )
        public override void Execute(Interpreter interp)
        {
            ArrayItem words = (ArrayItem)interp.StackPop();
            foreach (StringItem item in words.ArrayValue)
            {
                Word word;
                if (interp.TryFindWord(item.StringValue, out word))
                {
                    interp.CurModulePredecessor().AddWord(word);
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Can't find {0} to publish", item.StringValue));
                }
            }
        }
    }

    class ForWord : Word
    {
        public ForWord(string name) : base(name) { }

        // ( array forthic -- ? )
        public override void Execute(Interpreter interp)
        {
            StringItem forthic = (StringItem)interp.StackPop();
            ArrayItem indices = (ArrayItem)interp.StackPop();

            if (indices.ArrayValue.Count == 2)
            {
                IntItem i_max = (IntItem)indices.ArrayValue[0];
                IntItem j_max = (IntItem)indices.ArrayValue[1];
                for (var i = 0; i < i_max.IntValue; i++)
                {
                    for (var j = 0; j < j_max.IntValue; j++)
                    {
                        interp.StackPush(new IntItem(i));
                        interp.StackPush(new IntItem(j));
                        interp.Run(forthic.StringValue);
                    }
                }
            }
        }
    }



}
