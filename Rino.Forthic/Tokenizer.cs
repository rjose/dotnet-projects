using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Text;

namespace Rino.Forthic
{
    public class InvalidStateException : ApplicationException
    {
        public InvalidStateException(string message) : base(message) {}
    }

    public class Tokenizer
    {
        String inputString;
        int position;
        StringBuilder stringBuilder;
        HashSet<char> whitespace;
        char stringDelimiter;

        public Tokenizer(String str)
        {
            inputString = str;

            whitespace = new HashSet<char>();
            whitespace.Add(' ');
            whitespace.Add('\t');
            whitespace.Add('\r');
            whitespace.Add('\n');
            whitespace.Add('(');
            whitespace.Add(')');

            position = 0;
            stringDelimiter = '"';
            stringBuilder = new StringBuilder();
        }

        public Token NextToken()
        {
            stringBuilder.Clear();
            return transitionFromSTART();
        }

        public bool IsWhitespace(char c)
        {
            return whitespace.Contains(c);
        }

        public bool IsQuote(char c)
        {
            return (c == '"' || c == '\'');
        }

        public bool IsTripleQuote(int index, char c)
        {
            if (!IsQuote(c)) return false;
            if (index + 2 >= inputString.Length) return false;
            return (inputString[index+1] == c && inputString[index+2] == c);
        }

        // ---------------------------------------------------------------------
        // State Transition functions
        Token transitionFromSTART()
        {
            while (position < inputString.Length)
            {
                char c = inputString[position];

                if (IsWhitespace(c))
                {
                    position++;
                }
                else if (c == '#')
                {
                    position++;
                    return transitionFromCOMMENT();
                }
                else if (c == ':')
                {
                    position++;
                    return transitionFromSTART_DEFINITION();
                }
                else if (c == ';')
                {
                    position++;
                    return new EndDefinitionToken();
                }
                else if (c == '[')
                {
                    position++;
                    return new StartArrayToken();
                }
                else if (c == ']')
                {
                    position++;
                    return new EndArrayToken();
                }
                else if (c == '{')
                {
                    position++;
                    return transitionFromGATHER_MODULE();
                }
                else if (c == '}')
                {
                    position++;
                    return new EndModuleToken();
                }
                else if (IsTripleQuote(position, c))
                {
                    position += 3;  // Skip 3 quote chars
                    return transitionFromGATHER_TRIPLE_QUOTE_STRING(c);
                }
                else if (IsQuote(c))
                {
                    position++;
                    return transitionFromGATHER_STRING(c);
                }
                else
                {
                    return transitionFromGATHER_WORD();
                }
            }
            return new EOSToken();
        }


        Token transitionFromCOMMENT()
        {
            while (position < inputString.Length)
            {
                char c = inputString[position++];
                if (c == '\n') break;
            }
            return new CommentToken();
        }

        Token transitionFromSTART_DEFINITION()
        {
            while (position < inputString.Length)
            {
                char c = inputString[position++];

                if (IsWhitespace(c))
                {
                    continue;
                }
                else if (c == '"' || c == '\'')
                {
                    throw new InvalidStateException("Definition cannot start with a quote");
                }
                else
                {
                    position--;
                    return transitionFromGATHER_DEFINITION_NAME();
                }
            }
            throw new InvalidStateException("Got EOS in START_DEFINITION");
        }

        Token transitionFromGATHER_DEFINITION_NAME()
        {
            while (position < inputString.Length)
            {
                char c = inputString[position++];
                if (IsWhitespace(c))
                {
                    break;
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            return new StartDefinitionToken(stringBuilder.ToString());
        }

        Token transitionFromGATHER_MODULE()
        {
            while (position < inputString.Length)
            {
                char c = inputString[position++];
                if (IsWhitespace(c))
                {
                    break;
                }
                else if (c == '}')
                {
                    position--;
                    break;
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            return new StartModuleToken(stringBuilder.ToString());
        }

        Token transitionFromGATHER_TRIPLE_QUOTE_STRING(char delim)
        {
            stringDelimiter = delim;

            while (position < inputString.Length)
            {
                char c = inputString[position];
                if (c == stringDelimiter && IsTripleQuote(position, c))
                {
                    position += 3;
                    return new StringToken(stringBuilder.ToString());
                }
                else
                {
                    stringBuilder.Append(c);
                    position++;
                }
            }

            throw new InvalidStateException("Unterminated triple quote string");
        }

        Token transitionFromGATHER_STRING(char delim)
        {
            stringDelimiter = delim;

            while (position < inputString.Length)
            {
                char c = inputString[position++];
                if (c == stringDelimiter)
                {
                    return new StringToken(stringBuilder.ToString());
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            throw new InvalidStateException("Unterminated string");
        }

        Token transitionFromGATHER_WORD()
        {
            while (position < inputString.Length)
            {
                char c = inputString[position++];
                if (IsWhitespace(c))
                {
                    break;
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            return new WordToken(stringBuilder.ToString());
        }

    }
}
