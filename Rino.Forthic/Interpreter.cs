using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    public enum InterpreterMode { EXECUTING, COMPILING };

    /// <summary>
    /// </summary>
    public class Interpreter
    {
        public Stack<StackItem> stack { get; }
        protected List<Module> usingModules;
        protected List<Module> moduleStack;
        protected Dictionary<string, Module> registeredModules;
        protected InterpreterMode mode;
        protected DefinitionWord curDefinition;

        public Interpreter()
        {
            this.stack = new Stack<StackItem>();

            // Set up usingModules.
            usingModules = new List<Module>();

            // Set up moduleStack.
            // The first module is a local module that will always be in place during execution. 
            moduleStack = new List<Module>();
            ModuleStackPush(new Module(""));

            registeredModules = new Dictionary<string, Module>();
            mode = InterpreterMode.EXECUTING;


            // The first module (and the last to be searched) is the GlobalModule
            Module globalModule = new GlobalModule();
            UseModule(globalModule);
            runModuleCode(globalModule);
        }

        public void Run(string input)
        {
            Tokenizer tokenizer = new Tokenizer(input);
            Token tok = tokenizer.NextToken();
            while (tok.Type != TokenType.EOS)
            {
                HandleToken(tok);
                tok = tokenizer.NextToken();
            }
        }

        public void RegisterModule(Module module)
        {
            registeredModules.Add(module.Name, module);
            runModuleCode(module);
        }

        void runModuleCode(Module module)
        {
            ModuleStackPush(module);
            Run(module.Code);
            ModuleStackPop();
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
                result = new PushStackItemWord(name, module);
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

                case TokenType.START_DEFINITION:
                HandleStartDefinitionToken(token);
                break;

                case TokenType.END_DEFINITION:
                HandleEndDefinitionToken(token);
                break;

                case TokenType.COMMENT:
                break;

                default:
                throw new InvalidOperationException(String.Format("Unknown token: {0}", token));
            }
            return;
        }

        void HandleStartArrayToken(Token token)
        {
            handleWord(new PushStackItemWord("[", new StartArrayItem()));
        }

        void HandleEndArrayToken(Token token)
        {
            handleWord(new EndArrayWord());
        }

        void HandleWordToken(Token token)
        {
            WordToken wordToken = (WordToken)token;
            Word word;
            if (!TryFindWord(wordToken.Text, out word))
            {
                throw new ArgumentException(String.Format("Unknown word: {0}", wordToken.Text));
            }

            handleWord(word);
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
            handleWord(new PushStackItemWord("<string>", new StringItem(stringToken.Text)));
        }

        void HandleStartDefinitionToken(Token token)
        {
            if (mode == InterpreterMode.COMPILING)
            {
                throw new InvalidOperationException("Can't have nested definitions");
            }
            StartDefinitionToken sdToken = (StartDefinitionToken)token;
            curDefinition = new DefinitionWord(sdToken.Name);
            mode = InterpreterMode.COMPILING;
        }

        void HandleEndDefinitionToken(Token token)
        {
            if (mode == InterpreterMode.EXECUTING)
            {
                throw new InvalidOperationException("Unmatched end definition");
            }
            EndDefinitionToken edToken = (EndDefinitionToken)token;
            CurModule().AddWord(curDefinition);
            mode = InterpreterMode.EXECUTING;
        }

        void handleWord(Word word)
        {
            switch(mode)
            {
                case InterpreterMode.EXECUTING:
                word.Execute(this);
                break;

                case InterpreterMode.COMPILING:
                curDefinition.CompileWord(word);
                break;

                default:
                throw new InvalidOperationException(String.Format("Unknown mode {0}", mode));
            }
        }
    }
}
