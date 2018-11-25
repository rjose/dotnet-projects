using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class Interpreter
    {
        public Stack<StackItem> stack { get; }
        protected List<Module> usingModules;
        protected List<Module> moduleStack;
        protected Dictionary<string, Module> registeredModules;

        public Interpreter()
        {
            this.stack = new Stack<StackItem>();

            // Set up usingModules.
            // The first module (and the last to be searched) is the GlobalModule
            usingModules = new List<Module>();
            UseModule(new GlobalModule());

            // Set up moduleStack.
            // The first module is a local module that will always be in place during execution. 
            moduleStack = new List<Module>();
            ModuleStackPush(new Module(""));

            registeredModules = new Dictionary<string, Module>();
        }

        public void RegisterModule(Module module)
        {
            registeredModules.Add(module.Name, module);
        }

        public bool TryFindModule(string name, out Module result)
        {
            return registeredModules.TryGetValue(name, out result);
        }

        public bool TryFindWord(string name, out Word result)
        {
            bool found = false;

            // Search through moduleStack
            for (int i=moduleStack.Count-1; i >=0; i--)
            {
                found = moduleStack[i].TryFindWord(name, out result);
                if (found) return found;
            }

            // Search registered modules
            Module module;
            if (registeredModules.TryGetValue(name, out module)) {
                result = new PushModuleWord(name, module);
                return true;
            }

            // Search through usingModules
            for (int i=usingModules.Count-1; i >=0; i--)
            {
                found = usingModules[i].TryFindWord(name, out result);
                if (found) return found;
            }

            result = null;
            return false;
        }

        public Module CurModule()
        {
            return moduleStack[moduleStack.Count-1];
        }

        public void UseModule(Module module)
        {
            usingModules.Add(module);
        }

        public void ModuleStackPush(Module module)
        {
            moduleStack.Add(module);
        }

        public Module ModuleStackPop()
        {
            if (moduleStack.Count == 1)
            {
                throw new InvalidOperationException("Can't pop initial local module");
            }
            Module result = CurModule();
            moduleStack.RemoveAt(moduleStack.Count-1);
            return result;
        }

        public void StackPush(StackItem item)
        {
            this.stack.Push(item);
        }

        public StackItem StackPop()
        {
            return this.stack.Pop();
        }

        // ---------------------------------------------------------------------
        // Token handling

        public void HandleToken(Token token)
        {
            switch(token.Type)
            {
                case TokenType.START_ARRAY:
                HandleStartArrayToken(token);
                break;

                case TokenType.END_ARRAY:
                HandleEndArrayToken(token);
                break;

                case TokenType.WORD:
                HandleWordToken(token);
                break;

                case TokenType.START_MODULE:
                HandleStartModuleToken(token);
                break;

                case TokenType.END_MODULE:
                HandleEndModuleToken(token);
                break;

                case TokenType.STRING:
                HandleStringToken(token);
                break;

                default:
                throw new InvalidOperationException(String.Format("Unknown token: {0}", token));
            }
            return;
        }

        void HandleStartArrayToken(Token token)
        {
            StackPush(new StartArrayItem());
        }

        void HandleEndArrayToken(Token token)
        {
            List<StackItem> result = new List<StackItem>();
            for (int i=stack.Count-1; i >= 0; i--)
            {
                var item = StackPop();
                if (item is StartArrayItem) break;

                result.Add(item);
            }
            result.Reverse();
            StackPush(new ArrayItem(result));
        }

        void HandleWordToken(Token token)
        {
            WordToken wordToken = (WordToken)token;
            Word word;
            if (!TryFindWord(wordToken.Text, out word))
            {
                throw new ArgumentException(String.Format("Unknown word: {0}", wordToken.Text));
            }
            word.Execute(this);
        }

        void HandleStartModuleToken(Token token)
        {
            StartModuleToken startModuleToken = (StartModuleToken)token;
            Module module;

            // If module has been registered, push it onto the module stack
            if (TryFindModule(startModuleToken.Name, out module))
            {
                ModuleStackPush(module);
            }
            // If the module has no name, push an anonymous module
            else if (startModuleToken.Name == "")
            {
                ModuleStackPush(new Module(""));
            }
            // Register a new module under the specified name and push it onto the module stack
            else
            {
                module = new Module(startModuleToken.Name);
                RegisterModule(module);
                ModuleStackPush(module);
            }
        }

        void HandleEndModuleToken(Token token)
        {
            ModuleStackPop();
        }

        void HandleStringToken(Token token)
        {
            StringToken stringToken = (StringToken)token;
            StackPush(new StringItem(stringToken.Text));
        }
    }
}
